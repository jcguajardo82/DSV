(function() {
  var svg;

  //save off default references
  var d3 = window.d3, topojson = window.topojson;

  var defaultOptions = {
    scope: 'world',
    responsive: false,
    aspectRatio: 0.5625,
    setProjection: setProjection,
    projection: 'equirectangular',
    dataType: 'json',
    data: {},
    done: function() {},
    fills: {
      defaultFill: '#ABDDA4'
    },
    filters: {},
    geographyConfig: {
        dataUrl: null,
        hideAntarctica: true,
        hideHawaiiAndAlaska : false,
        borderWidth: 1,
        borderColor: '#FDFDFD',
        popupTemplate: function(geography, data) {
          return '<div class="hoverinfo"><strong>' + geography.properties.name + '</strong></div>';
        },
        popupOnHover: true,
        highlightOnHover: true,
        highlightFillColor: '#FC8D59',
        highlightBorderColor: 'rgba(250, 15, 160, 0.2)',
        highlightBorderWidth: 2
    },
    projectionConfig: {
      rotation: [97, 0]
    },
    bubblesConfig: {
        borderWidth: 2,
        borderColor: '#FFFFFF',
        popupOnHover: true,
        radius: null,
        popupTemplate: function(geography, data) {
          return '<div class="hoverinfo"><strong>' + data.name + '</strong></div>';
        },
        fillOpacity: 0.75,
        animate: true,
        highlightOnHover: true,
        highlightFillColor: '#FC8D59',
        highlightBorderColor: 'rgba(250, 15, 160, 0.2)',
        highlightBorderWidth: 2,
        highlightFillOpacity: 0.85,
        exitDelay: 100,
        key: JSON.stringify
    },
    arcConfig: {
      strokeColor: '#DD1C77',
      strokeWidth: 1,
      arcSharpness: 1,
      animationSpeed: 600
    }
  };

  /*
    Getter for value. If not declared on datumValue, look up the chain into optionsValue
  */
  function val( datumValue, optionsValue, context ) {
    if ( typeof context === 'undefined' ) {
      context = optionsValue;
      optionsValues = undefined;
    }
    var value = typeof datumValue !== 'undefined' ? datumValue : optionsValue;

    if (typeof value === 'undefined') {
      return  null;
    }

    if ( typeof value === 'function' ) {
      var fnContext = [context];
      if ( context.geography ) {
        fnContext = [context.geography, context.data];
      }
      return value.apply(null, fnContext);
    }
    else {
      return value;
    }
  }

  function addContainer( element, height, width ) {
    this.svg = d3.select( element ).append('svg')
      .attr('width', width || element.offsetWidth)
      .attr('data-width', width || element.offsetWidth)
      .attr('class', 'datamap')
      .attr('height', height || element.offsetHeight)
      .style('overflow', 'hidden'); // IE10+ doesn't respect height/width when map is zoomed in

    if (this.options.responsive) {
      d3.select(this.options.element).style({'position': 'relative', 'padding-bottom': (this.options.aspectRatio*100) + '%'});
      d3.select(this.options.element).select('svg').style({'position': 'absolute', 'width': '100%', 'height': '100%'});
      d3.select(this.options.element).select('svg').select('g').selectAll('path').style('vector-effect', 'non-scaling-stroke');

    }

    return this.svg;
  }

  // setProjection takes the svg element and options
  function setProjection( element, options ) {
    var width = options.width || element.offsetWidth;
    var height = options.height || element.offsetHeight;
    var projection, path;
    var svg = this.svg;

    if ( options && typeof options.scope === 'undefined') {
      options.scope = 'world';
    }

    if ( options.scope === 'usa' ) {
      projection = d3.geo.albersUsa()
        .scale(width)
        .translate([width / 2, height / 2]);
    }
    else if ( options.scope === 'world' ) {
      projection = d3.geo[options.projection]()
        .scale((width + 1) / 2 / Math.PI)
        .translate([width / 2, height / (options.projection === "mercator" ? 1.45 : 1.8)]);
    }

    if ( options.projection === 'orthographic' ) {

      svg.append("defs").append("path")
        .datum({type: "Sphere"})
        .attr("id", "sphere")
        .attr("d", path);

      svg.append("use")
          .attr("class", "stroke")
          .attr("xlink:href", "#sphere");

      svg.append("use")
          .attr("class", "fill")
          .attr("xlink:href", "#sphere");
      projection.scale(250).clipAngle(90).rotate(options.projectionConfig.rotation)
    }

    path = d3.geo.path()
      .projection( projection );

    return {path: path, projection: projection};
  }

  function addStyleBlock() {
    if ( d3.select('.datamaps-style-block').empty() ) {
      d3.select('head').append('style').attr('class', 'datamaps-style-block')
      .html('.datamap path.datamaps-graticule { fill: none; stroke: #777; stroke-width: 0.5px; stroke-opacity: .5; pointer-events: none; } .datamap .labels {pointer-events: none;} .datamap path {stroke: #FFFFFF; stroke-width: 1px;} .datamaps-legend dt, .datamaps-legend dd { float: left; margin: 0 3px 0 0;} .datamaps-legend dd {width: 20px; margin-right: 6px; border-radius: 3px;} .datamaps-legend {padding-bottom: 20px; z-index: 1001; position: absolute; left: 4px; font-size: 12px; font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;} .datamaps-hoverover {display: none; font-family: "Helvetica Neue", Helvetica, Arial, sans-serif; } .hoverinfo {padding: 4px; border-radius: 1px; background-color: #FFF; box-shadow: 1px 1px 5px #CCC; font-size: 12px; border: 1px solid #CCC; } .hoverinfo hr {border:1px dotted #CCC; }');
    }
  }

  function drawSubunits( data ) {
    var fillData = this.options.fills,
        colorCodeData = this.options.data || {},
        geoConfig = this.options.geographyConfig;


    var subunits = this.svg.select('g.datamaps-subunits');
    if ( subunits.empty() ) {
      subunits = this.addLayer('datamaps-subunits', null, true);
    }

    var geoData = topojson.feature( data, data.objects[ this.options.scope ] ).features;
    if ( geoConfig.hideAntarctica ) {
      geoData = geoData.filter(function(feature) {
        return feature.id !== "ATA";
      });
    }

    if ( geoConfig.hideHawaiiAndAlaska ) {
      geoData = geoData.filter(function(feature) {
        return feature.id !== "HI" && feature.id !== 'AK';
      });
    }

    var geo = subunits.selectAll('path.datamaps-subunit').data( geoData );

    geo.enter()
      .append('path')
      .attr('d', this.path)
      .attr('class', function(d) {
        return 'datamaps-subunit ' + d.id;
      })
      .attr('data-info', function(d) {
        return JSON.stringify( colorCodeData[d.id]);
      })
      .style('fill', function(d) {
        //if fillKey - use that
        //otherwise check 'fill'
        //otherwise check 'defaultFill'
        var fillColor;

        var datum = colorCodeData[d.id];
        if ( datum && datum.fillKey ) {
          fillColor = fillData[ val(datum.fillKey, {data: colorCodeData[d.id], geography: d}) ];
        }

        if ( typeof fillColor === 'undefined' ) {
          fillColor = val(datum && datum.fillColor, fillData.defaultFill, {data: colorCodeData[d.id], geography: d});
        }

        return fillColor;
      })
      .style('stroke-width', geoConfig.borderWidth)
      .style('stroke', geoConfig.borderColor);
  }

  function handleGeographyConfig () {
    var hoverover;
    var svg = this.svg;
    var self = this;
    var options = this.options.geographyConfig;

    if ( options.highlightOnHover || options.popupOnHover ) {
      svg.selectAll('.datamaps-subunit')
        .on('mouseover', function(d) {
          var $this = d3.select(this);
          var datum = self.options.data[d.id] || {};
          if ( options.highlightOnHover ) {
            var previousAttributes = {
              'fill':  $this.style('fill'),
              'stroke': $this.style('stroke'),
              'stroke-width': $this.style('stroke-width'),
              'fill-opacity': $this.style('fill-opacity')
            };

            $this
              .style('fill', val(datum.highlightFillColor, options.highlightFillColor, datum))
              .style('stroke', val(datum.highlightBorderColor, options.highlightBorderColor, datum))
              .style('stroke-width', val(datum.highlightBorderWidth, options.highlightBorderWidth, datum))
              .style('fill-opacity', val(datum.highlightFillOpacity, options.highlightFillOpacity, datum))
              .attr('data-previousAttributes', JSON.stringify(previousAttributes));

            //as per discussion on https://github.com/markmarkoh/datamaps/issues/19
            if ( ! /((MSIE)|(Trident))/.test(navigator.userAgent) ) {
             moveToFront.call(this);
            }
          }

          if ( options.popupOnHover ) {
            self.updatePopup($this, d, options, svg);
          }
        })
        .on('mouseout', function() {
          var $this = d3.select(this);

          if (options.highlightOnHover) {
            //reapply previous attributes
            var previousAttributes = JSON.parse( $this.attr('data-previousAttributes') );
            for ( var attr in previousAttributes ) {
              $this.style(attr, previousAttributes[attr]);
            }
          }
          $this.on('mousemove', null);
          d3.selectAll('.datamaps-hoverover').style('display', 'none');
        });
    }

    function moveToFront() {
      this.parentNode.appendChild(this);
    }
  }

  //plugin to add a simple map legend
  function addLegend(layer, data, options) {
    data = data || {};
    if ( !this.options.fills ) {
      return;
    }

    var html = '<dl>';
    var label = '';
    if ( data.legendTitle ) {
      html = '<h2>' + data.legendTitle + '</h2>' + html;
    }
    for ( var fillKey in this.options.fills ) {

      if ( fillKey === 'defaultFill') {
        if (! data.defaultFillName ) {
          continue;
        }
        label = data.defaultFillName;
      } else {
        if (data.labels && data.labels[fillKey]) {
          label = data.labels[fillKey];
        } else {
          label= fillKey + ': ';
        }
      }
      html += '<dt>' + label + '</dt>';
      html += '<dd style="background-color:' +  this.options.fills[fillKey] + '">&nbsp;</dd>';
    }
    html += '</dl>';

    var hoverover = d3.select( this.options.element ).append('div')
      .attr('class', 'datamaps-legend')
      .html(html);
  }

    function addGraticule ( layer, options ) {
      var graticule = d3.geo.graticule();
      this.svg.insert("path", '.datamaps-subunits')
        .datum(graticule)
        .attr("class", "datamaps-graticule")
        .attr("d", this.path);
  }

  function handleArcs (layer, data, options) {
    var self = this,
        svg = this.svg;

    if ( !data || (data && !data.slice) ) {
      throw "Datamaps Error - arcs must be an array";
    }

    // For some reason arc options were put in an `options` object instead of the parent arc
    // I don't like this, so to match bubbles and other plugins I'm moving it
    // This is to keep backwards compatability
    for ( var i = 0; i < data.length; i++ ) {
      data[i] = defaults(data[i], data[i].options);
      delete data[i].options;
    }

    if ( typeof options === "undefined" ) {
      options = defaultOptions.arcConfig;
    }

    var arcs = layer.selectAll('path.datamaps-arc').data( data, JSON.stringify );

    var path = d3.geo.path()
        .projection(self.projection);

    arcs
      .enter()
        .append('svg:path')
        .attr('class', 'datamaps-arc')
        .style('stroke-linecap', 'round')
        .style('stroke', function(datum) {
          return val(datum.strokeColor, options.strokeColor, datum);
        })
        .style('fill', 'none')
        .style('stroke-width', function(datum) {
            return val(datum.strokeWidth, options.strokeWidth, datum);
        })
        .attr('d', function(datum) {
            var originXY = self.latLngToXY(val(datum.origin.latitude, datum), val(datum.origin.longitude, datum))
            var destXY = self.latLngToXY(val(datum.destination.latitude, datum), val(datum.destination.longitude, datum));
            var midXY = [ (originXY[0] + destXY[0]) / 2, (originXY[1] + destXY[1]) / 2];
            if (options.greatArc) {
                  // TODO: Move this to inside `if` clause when setting attr `d`
              var greatArc = d3.geo.greatArc()
                  .source(function(d) { return [val(d.origin.longitude, d), val(d.origin.latitude, d)]; })
                  .target(function(d) { return [val(d.destination.longitude, d), val(d.destination.latitude, d)]; });

              return path(greatArc(datum))
            }
            var sharpness = val(datum.arcSharpness, options.arcSharpness, datum);
            return "M" + originXY[0] + ',' + originXY[1] + "S" + (midXY[0] + (50 * sharpness)) + "," + (midXY[1] - (75 * sharpness)) + "," + destXY[0] + "," + destXY[1];
        })
        .transition()
          .delay(100)
          .style('fill', function(datum) {
            /*
              Thank you Jake Archibald, this is awesome.
              Source: http://jakearchibald.com/2013/animated-line-drawing-svg/
            */
            var length = this.getTotalLength();
            this.style.transition = this.style.WebkitTransition = 'none';
            this.style.strokeDasharray = length + ' ' + length;
            this.style.strokeDashoffset = length;
            this.getBoundingClientRect();
            this.style.transition = this.style.WebkitTransition = 'stroke-dashoffset ' + val(datum.animationSpeed, options.animationSpeed, datum) + 'ms ease-out';
            this.style.strokeDashoffset = '0';
            return 'none';
          })

    arcs.exit()
      .transition()
      .style('opacity', 0)
      .remove();
  }

  function handleLabels ( layer, options ) {
    var self = this;
    options = options || {};
    var labelStartCoodinates = this.projection([-67.707617, 42.722131]);
    this.svg.selectAll(".datamaps-subunit")
      .attr("data-foo", function(d) {
        var center = self.path.centroid(d);
        var xOffset = 7.5, yOffset = 5;

        if ( ["FL", "KY", "MI"].indexOf(d.id) > -1 ) xOffset = -2.5;
        if ( d.id === "NY" ) xOffset = -1;
        if ( d.id === "MI" ) yOffset = 18;
        if ( d.id === "LA" ) xOffset = 13;

        var x,y;

        x = center[0] - xOffset;
        y = center[1] + yOffset;

        var smallStateIndex = ["VT", "NH", "MA", "RI", "CT", "NJ", "DE", "MD", "DC"].indexOf(d.id);
        if ( smallStateIndex > -1) {
          var yStart = labelStartCoodinates[1];
          x = labelStartCoodinates[0];
          y = yStart + (smallStateIndex * (2+ (options.fontSize || 12)));
          layer.append("line")
            .attr("x1", x - 3)
            .attr("y1", y - 5)
            .attr("x2", center[0])
            .attr("y2", center[1])
            .style("stroke", options.labelColor || "#000")
            .style("stroke-width", options.lineWidth || 1)
        }

        layer.append("text")
          .attr("x", x)
          .attr("y", y)
          .style("font-size", (options.fontSize || 10) + 'px')
          .style("font-family", options.fontFamily || "Verdana")
          .style("fill", options.labelColor || "#000")
          .text( d.id );
        return "bar";
      });
  }


  function handleBubbles (layer, data, options ) {
    var self = this,
        fillData = this.options.fills,
        filterData = this.options.filters,
        svg = this.svg;

    if ( !data || (data && !data.slice) ) {
      throw "Datamaps Error - bubbles must be an array";
    }

    var bubbles = layer.selectAll('circle.datamaps-bubble').data( data, options.key );

    bubbles
      .enter()
        .append('svg:circle')
        .attr('class', 'datamaps-bubble')
        .attr('cx', function ( datum ) {
          var latLng;
          if ( datumHasCoords(datum) ) {
            latLng = self.latLngToXY(datum.latitude, datum.longitude);
          }
          else if ( datum.centered ) {
            latLng = self.path.centroid(svg.select('path.' + datum.centered).data()[0]);
          }
          if ( latLng ) return latLng[0];
        })
        .attr('cy', function ( datum ) {
          var latLng;
          if ( datumHasCoords(datum) ) {
            latLng = self.latLngToXY(datum.latitude, datum.longitude);
          }
          else if ( datum.centered ) {
            latLng = self.path.centroid(svg.select('path.' + datum.centered).data()[0]);
          }
          if ( latLng ) return latLng[1];
        })
        .attr('r', function(datum) {
          // if animation enabled start with radius 0, otherwise use full size.
          return options.animate ? 0 : val(datum.radius, options.radius, datum);
        })
        .attr('data-info', function(d) {
          return JSON.stringify(d);
        })
        .attr('filter', function (datum) {
          var filterKey = filterData[ val(datum.filterKey, options.filterKey, datum) ];

          if (filterKey) {
            return filterKey;
          }
        })
        .style('stroke', function ( datum ) {
          return val(datum.borderColor, options.borderColor, datum);
        })
        .style('stroke-width', function ( datum ) {
          return val(datum.borderWidth, options.borderWidth, datum);
        })
        .style('fill-opacity', function ( datum ) {
          return val(datum.fillOpacity, options.fillOpacity, datum);
        })
        .style('fill', function ( datum ) {
          var fillColor = fillData[ val(datum.fillKey, options.fillKey, datum) ];
          return fillColor || fillData.defaultFill;
        })
        .on('mouseover', function ( datum ) {
          var $this = d3.select(this);

          if (options.highlightOnHover) {
            //save all previous attributes for mouseout
            var previousAttributes = {
              'fill':  $this.style('fill'),
              'stroke': $this.style('stroke'),
              'stroke-width': $this.style('stroke-width'),
              'fill-opacity': $this.style('fill-opacity')
            };

            $this
              .style('fill', val(datum.highlightFillColor, options.highlightFillColor, datum))
              .style('stroke', val(datum.highlightBorderColor, options.highlightBorderColor, datum))
              .style('stroke-width', val(datum.highlightBorderWidth, options.highlightBorderWidth, datum))
              .style('fill-opacity', val(datum.highlightFillOpacity, options.highlightFillOpacity, datum))
              .attr('data-previousAttributes', JSON.stringify(previousAttributes));
          }

          if (options.popupOnHover) {
            self.updatePopup($this, datum, options, svg);
          }
        })
        .on('mouseout', function ( datum ) {
          var $this = d3.select(this);

          if (options.highlightOnHover) {
            //reapply previous attributes
            var previousAttributes = JSON.parse( $this.attr('data-previousAttributes') );
            for ( var attr in previousAttributes ) {
              $this.style(attr, previousAttributes[attr]);
            }
          }

          d3.selectAll('.datamaps-hoverover').style('display', 'none');
        })

    bubbles.transition()
      .duration(400)
      .attr('r', function ( datum ) {
        return val(datum.radius, options.radius, datum);
      });

    bubbles.exit()
      .transition()
        .delay(options.exitDelay)
        .attr("r", 0)
        .remove();

    function datumHasCoords (datum) {
      return typeof datum !== 'undefined' && typeof datum.latitude !== 'undefined' && typeof datum.longitude !== 'undefined';
    }
  }

  //stolen from underscore.js
  function defaults(obj) {
    Array.prototype.slice.call(arguments, 1).forEach(function(source) {
      if (source) {
        for (var prop in source) {
          if (obj[prop] == null) obj[prop] = source[prop];
        }
      }
    });
    return obj;
  }
  /**************************************
             Public Functions
  ***************************************/

  function Datamap( options ) {

    if ( typeof d3 === 'undefined' || typeof topojson === 'undefined' ) {
      throw new Error('Include d3.js (v3.0.3 or greater) and topojson on this page before creating a new map');
   }
    //set options for global use
    this.options = defaults(options, defaultOptions);
    this.options.geographyConfig = defaults(options.geographyConfig, defaultOptions.geographyConfig);
    this.options.projectionConfig = defaults(options.projectionConfig, defaultOptions.projectionConfig);
    this.options.bubblesConfig = defaults(options.bubblesConfig, defaultOptions.bubblesConfig);
    this.options.arcConfig = defaults(options.arcConfig, defaultOptions.arcConfig);

    //add the SVG container
    if ( d3.select( this.options.element ).select('svg').length > 0 ) {
      addContainer.call(this, this.options.element, this.options.height, this.options.width );
    }

    /* Add core plugins to this instance */
    this.addPlugin('bubbles', handleBubbles);
    this.addPlugin('legend', addLegend);
    this.addPlugin('arc', handleArcs);
    this.addPlugin('labels', handleLabels);
    this.addPlugin('graticule', addGraticule);

    //append style block with basic hoverover styles
    if ( ! this.options.disableDefaultStyles ) {
      addStyleBlock();
    }

    return this.draw();
  }

  // resize map
  Datamap.prototype.resize = function () {

    var self = this;
    var options = self.options;

    if (options.responsive) {
      var newsize = options.element.clientWidth,
          oldsize = d3.select( options.element).select('svg').attr('data-width');

      d3.select(options.element).select('svg').selectAll('g').attr('transform', 'scale(' + (newsize / oldsize) + ')');
    }
  }

  // actually draw the features(states & countries)
  Datamap.prototype.draw = function() {
    //save off in a closure
    var self = this;
    var options = self.options;

    //set projections and paths based on scope
    var pathAndProjection = options.setProjection.apply(self, [options.element, options] );

    this.path = pathAndProjection.path;
    this.projection = pathAndProjection.projection;

    //if custom URL for topojson data, retrieve it and render
    if ( options.geographyConfig.dataUrl ) {
      d3.json( options.geographyConfig.dataUrl, function(error, results) {
        if ( error ) throw new Error(error);
        self.customTopo = results;
        draw( results );
      });
    }
    else {
      draw( this[options.scope + 'Topo'] || options.geographyConfig.dataJson);
    }

    return this;

      function draw (data) {
        // if fetching remote data, draw the map first then call `updateChoropleth`
        if ( self.options.dataUrl ) {
          //allow for csv or json data types
          d3[self.options.dataType](self.options.dataUrl, function(data) {
            //in the case of csv, transform data to object
            if ( self.options.dataType === 'csv' && (data && data.slice) ) {
              var tmpData = {};
              for(var i = 0; i < data.length; i++) {
                tmpData[data[i].id] = data[i];
              }
              data = tmpData;
            }
            Datamaps.prototype.updateChoropleth.call(self, data);
          });
        }
        drawSubunits.call(self, data);
        handleGeographyConfig.call(self);

        if ( self.options.geographyConfig.popupOnHover || self.options.bubblesConfig.popupOnHover) {
          hoverover = d3.select( self.options.element ).append('div')
            .attr('class', 'datamaps-hoverover')
            .style('z-index', 10001)
            .style('position', 'absolute');
        }

        //fire off finished callback
        self.options.done(self);
      }
  };
  /**************************************
                TopoJSON
  ***************************************/
  Datamap.prototype.worldTopo = '__WORLD__';
  Datamap.prototype.abwTopo = '__ABW__';
  Datamap.prototype.afgTopo = '__AFG__';
  Datamap.prototype.agoTopo = '__AGO__';
  Datamap.prototype.aiaTopo = '__AIA__';
  Datamap.prototype.albTopo = '__ALB__';
  Datamap.prototype.aldTopo = '__ALD__';
  Datamap.prototype.andTopo = '__AND__';
  Datamap.prototype.areTopo = '__ARE__';
  Datamap.prototype.argTopo = '__ARG__';
  Datamap.prototype.armTopo = '__ARM__';
  Datamap.prototype.asmTopo = '__ASM__';
  Datamap.prototype.ataTopo = '__ATA__';
  Datamap.prototype.atcTopo = '__ATC__';
  Datamap.prototype.atfTopo = '__ATF__';
  Datamap.prototype.atgTopo = '__ATG__';
  Datamap.prototype.ausTopo = '__AUS__';
  Datamap.prototype.autTopo = '__AUT__';
  Datamap.prototype.azeTopo = '__AZE__';
  Datamap.prototype.bdiTopo = '__BDI__';
  Datamap.prototype.belTopo = '__BEL__';
  Datamap.prototype.benTopo = '__BEN__';
  Datamap.prototype.bfaTopo = '__BFA__';
  Datamap.prototype.bgdTopo = '__BGD__';
  Datamap.prototype.bgrTopo = '__BGR__';
  Datamap.prototype.bhrTopo = '__BHR__';
  Datamap.prototype.bhsTopo = '__BHS__';
  Datamap.prototype.bihTopo = '__BIH__';
  Datamap.prototype.bjnTopo = '__BJN__';
  Datamap.prototype.blmTopo = '__BLM__';
  Datamap.prototype.blrTopo = '__BLR__';
  Datamap.prototype.blzTopo = '__BLZ__';
  Datamap.prototype.bmuTopo = '__BMU__';
  Datamap.prototype.bolTopo = '__BOL__';
  Datamap.prototype.braTopo = '__BRA__';
  Datamap.prototype.brbTopo = '__BRB__';
  Datamap.prototype.brnTopo = '__BRN__';
  Datamap.prototype.btnTopo = '__BTN__';
  Datamap.prototype.norTopo = '__NOR__';
  Datamap.prototype.bwaTopo = '__BWA__';
  Datamap.prototype.cafTopo = '__CAF__';
  Datamap.prototype.canTopo = '__CAN__';
  Datamap.prototype.cheTopo = '__CHE__';
  Datamap.prototype.chlTopo = '__CHL__';
  Datamap.prototype.chnTopo = '__CHN__';
  Datamap.prototype.civTopo = '__CIV__';
  Datamap.prototype.clpTopo = '__CLP__';
  Datamap.prototype.cmrTopo = '__CMR__';
  Datamap.prototype.codTopo = '__COD__';
  Datamap.prototype.cogTopo = '__COG__';
  Datamap.prototype.cokTopo = '__COK__';
  Datamap.prototype.colTopo = '__COL__';
  Datamap.prototype.comTopo = '__COM__';
  Datamap.prototype.cpvTopo = '__CPV__';
  Datamap.prototype.criTopo = '__CRI__';
  Datamap.prototype.csiTopo = '__CSI__';
  Datamap.prototype.cubTopo = '__CUB__';
  Datamap.prototype.cuwTopo = '__CUW__';
  Datamap.prototype.cymTopo = '__CYM__';
  Datamap.prototype.cynTopo = '__CYN__';
  Datamap.prototype.cypTopo = '__CYP__';
  Datamap.prototype.czeTopo = '__CZE__';
  Datamap.prototype.deuTopo = '__DEU__';
  Datamap.prototype.djiTopo = '__DJI__';
  Datamap.prototype.dmaTopo = '__DMA__';
  Datamap.prototype.dnkTopo = '__DNK__';
  Datamap.prototype.domTopo = '__DOM__';
  Datamap.prototype.dzaTopo = '__DZA__';
  Datamap.prototype.ecuTopo = '__ECU__';
  Datamap.prototype.egyTopo = '__EGY__';
  Datamap.prototype.eriTopo = '__ERI__';
  Datamap.prototype.esbTopo = '__ESB__';
  Datamap.prototype.espTopo = '__ESP__';
  Datamap.prototype.estTopo = '__EST__';
  Datamap.prototype.ethTopo = '__ETH__';
  Datamap.prototype.finTopo = '__FIN__';
  Datamap.prototype.fjiTopo = '__FJI__';
  Datamap.prototype.flkTopo = '__FLK__';
  Datamap.prototype.fraTopo = '__FRA__';
  Datamap.prototype.froTopo = '__FRO__';
  Datamap.prototype.fsmTopo = '__FSM__';
  Datamap.prototype.gabTopo = '__GAB__';
  Datamap.prototype.psxTopo = '__PSX__';
  Datamap.prototype.gbrTopo = '__GBR__';
  Datamap.prototype.geoTopo = '__GEO__';
  Datamap.prototype.ggyTopo = '__GGY__';
  Datamap.prototype.ghaTopo = '__GHA__';
  Datamap.prototype.gibTopo = '__GIB__';
  Datamap.prototype.ginTopo = {"type":"Topology","objects":{"gin":{"type":"GeometryCollection","geometries":[{"type":"Polygon","properties":{"name":"Beyla"},"id":"GN.BE","arcs":[[0,1,2,3,4,5]]},{"type":"Polygon","properties":{"name":"Boffa"},"id":"GN.BF","arcs":[[6,7,8,9,10]]},{"type":"Polygon","properties":{"name":"Boke"},"id":"GN.BK","arcs":[[11,12,-10,13]]},{"type":"Polygon","properties":{"name":"Conakry"},"id":"GN.CK","arcs":[[14,15]]},{"type":"Polygon","properties":{"name":"Coyah"},"id":"GN.CO","arcs":[[16,17,18,19]]},{"type":"Polygon","properties":{"name":"Dabola"},"id":"GN.DB","arcs":[[20,21,22,23,24]]},{"type":"Polygon","properties":{"name":"Dalaba"},"id":"GN.DL","arcs":[[25,26,27,28,29,30]]},{"type":"Polygon","properties":{"name":"Dinguiraye"},"id":"GN.DI","arcs":[[31,32,-24,33,34,35]]},{"type":"Polygon","properties":{"name":"Dubréka"},"id":"GN.DU","arcs":[[-19,36,-16,37,-8,38,39,40]]},{"type":"Polygon","properties":{"name":"Faranah"},"id":"GN.FA","arcs":[[41,42,43,-21,44]]},{"type":"Polygon","properties":{"name":"Forécariah"},"id":"GN.FO","arcs":[[45,-17,46]]},{"type":"Polygon","properties":{"name":"Fria"},"id":"GN.FR","arcs":[[-39,-7,47]]},{"type":"Polygon","properties":{"name":"Gaoual"},"id":"GN.GA","arcs":[[48,49,50,-12,51,52]]},{"type":"Polygon","properties":{"name":"Guéckédou"},"id":"GN.GU","arcs":[[53,54,-43,55]]},{"type":"Polygon","properties":{"name":"Kankan"},"id":"GN.KA","arcs":[[56,-5,57,58,59,60]]},{"type":"Polygon","properties":{"name":"Kérouané"},"id":"GN.KE","arcs":[[-4,61,62,-58]]},{"type":"Polygon","properties":{"name":"Kindia"},"id":"GN.KD","arcs":[[63,64,-47,-20,-41,65,66,-29]]},{"type":"Polygon","properties":{"name":"Kissidougou"},"id":"GN.KS","arcs":[[-59,-63,67,-56,-42,68]]},{"type":"Polygon","properties":{"name":"Koubia"},"id":"GN.KB","arcs":[[69,-26,70,71,72]]},{"type":"Polygon","properties":{"name":"Koundara"},"id":"GN.KN","arcs":[[73,-53,74]]},{"type":"Polygon","properties":{"name":"Kouroussa"},"id":"GN.KO","arcs":[[75,76,-60,-69,-45,-25,-33]]},{"type":"Polygon","properties":{"name":"Labé"},"id":"GN.LA","arcs":[[-31,77,78,79,-71]]},{"type":"Polygon","properties":{"name":"Lélouma"},"id":"GN.LE","arcs":[[80,81,-50,82,-79]]},{"type":"Polygon","properties":{"name":"Lola"},"id":"GN.LO","arcs":[[83,84,-1]]},{"type":"Polygon","properties":{"name":"Macenta"},"id":"GN.MC","arcs":[[-3,85,86,87,-54,-68,-62]]},{"type":"Polygon","properties":{"name":"Mali"},"id":"GN.ML","arcs":[[-72,-80,-83,-49,-74,88]]},{"type":"Polygon","properties":{"name":"Mamou"},"id":"GN.MM","arcs":[[-23,89,-64,-28,90,-34]]},{"type":"Polygon","properties":{"name":"Mandiana"},"id":"GN.MD","arcs":[[-61,-77,91,92]]},{"type":"Polygon","properties":{"name":"Nzérékoré"},"id":"GN.NZ","arcs":[[-85,93,94,-86,-2]]},{"type":"Polygon","properties":{"name":"Pita"},"id":"GN.PI","arcs":[[-78,-30,-67,95,-81]]},{"type":"Polygon","properties":{"name":"Siguiri"},"id":"GN.SI","arcs":[[-92,-76,-32,96]]},{"type":"Polygon","properties":{"name":"Tougué"},"id":"GN.TO","arcs":[[-35,-91,-27,-70,97]]},{"type":"Polygon","properties":{"name":"Yomou"},"id":"GN.YO","arcs":[[98,-87,-95]]},{"type":"Polygon","properties":{"name":"Télimélé"},"id":"GN.TE","arcs":[[-82,-96,-66,-40,-48,-11,-13,-51]]}]}},"arcs":[[[9453,1515],[-59,51],[-129,158],[-98,110],[-121,44],[-81,-6]],[[8965,1872],[-20,44],[-45,44],[-81,11],[-16,66],[-53,76],[-85,28],[-113,-11],[-118,-71],[-117,-39]],[[8317,2020],[20,77],[29,104],[24,121],[-20,159],[-45,153],[-65,120],[-20,83],[-2,394]],[[8238,3231],[57,73],[58,36],[104,26],[57,36],[49,30],[1,10],[2,10],[12,30],[2,19],[5,17],[60,138],[61,58],[31,78],[46,83],[46,21],[12,73],[57,26]],[[8898,3995],[69,-6],[58,-26],[88,-51],[66,10],[80,57],[85,52],[105,-6]],[[9449,4025],[22,-15],[30,-8],[30,-2],[24,3],[28,14],[51,39],[29,8],[55,0],[7,-16],[-35,-145],[-4,-43],[-48,-158],[1,-66],[69,-37],[16,4],[14,8],[11,-3],[18,-47],[15,-20],[30,-31],[56,-43],[17,-27],[-2,-2],[-21,-22],[-32,-6],[-27,0],[-26,-4],[-121,-83],[-20,-29],[-13,-44],[-7,-43],[-6,-90],[-24,-158],[13,-56],[62,-25],[30,-5],[81,-33],[6,1],[14,10],[8,0],[9,-8],[10,-23],[7,-10],[25,-30],[10,-16],[32,-78],[12,-20],[38,-34],[10,-16],[7,-34],[-3,-70],[3,-38],[16,-76],[0,-32],[-9,-46],[-5,-43],[9,-39],[28,-78],[-79,-11],[-37,7],[-30,26],[-43,114],[-29,52],[-29,-6],[-5,-28],[4,-30],[-3,-26],[-23,-14],[-22,1],[-22,9],[-20,14],[-14,18],[-11,24],[-7,26],[-9,24],[-17,18],[-19,5],[-21,-4],[-40,-14],[-13,3],[-23,-3],[-11,1],[-9,6],[-15,17],[-11,5],[-28,-2],[-44,-24],[-28,-4],[-48,13],[-19,-3],[-65,-70],[-8,-13],[0,-22],[7,-19],[10,-21],[7,-23],[2,-37],[-17,-104],[0,-34],[3,-34],[-4,-21],[-22,-32],[-1,-19],[111,-99],[132,-54],[13,3],[81,37],[17,0],[1,-17],[-8,-26],[-10,-22],[-8,-56],[0,-27],[8,-26],[25,-20],[22,-34],[17,-40],[3,-18],[4,-22],[-23,3],[-17,14],[-13,15],[-13,6],[-57,-13],[-20,1],[2,-15]],[[1980,6427],[-17,-292],[-64,-66],[-25,-22],[-56,-27],[-33,-66],[-73,-110],[-24,-76],[13,-104]],[[1701,5664],[-30,-104],[-38,-98],[-52,-103],[-5,-9],[-2,-13],[4,-23],[4,-13],[5,-12],[6,-10],[154,-174],[6,-5],[7,-3],[16,-4],[53,-1],[8,-2],[14,-8],[11,-14],[13,-31],[0,-3],[-1,0]],[[1874,5034],[-4,6],[-5,-25],[-2,-35],[-4,-17],[-15,-12],[-26,-52],[-19,-11],[1,-8],[-6,-5],[-47,-24],[-5,-1],[-28,3],[-25,7],[-20,10],[-9,12],[-7,21],[-19,32],[-23,29],[-20,12],[-36,31],[-8,14],[-16,43],[-12,7],[-23,2],[-11,4],[-10,11],[-27,38],[-20,11],[-14,3],[-15,7],[-20,23],[-5,45],[21,54],[35,27],[36,-33],[-10,22],[-19,30],[-18,22],[-8,-5],[-10,-23],[-20,9],[-16,21],[4,12],[29,16],[23,38],[36,83],[-19,-13],[-14,-16],[-27,-39],[-15,-13],[-36,-23],[-9,-8],[-3,-22],[3,-84],[-4,-21],[-11,-19],[-18,-13],[-22,-4],[-31,12],[-24,28],[-12,36],[1,36],[22,30],[33,32],[20,33],[-20,31],[-5,-27],[-15,-15],[-21,-7],[-22,-1],[3,-41],[-24,-31],[-30,-12],[-14,15],[-5,12],[-9,16],[-10,20],[-4,27],[5,13],[8,12],[6,11],[-6,13],[-7,4],[-7,-3],[-6,-6],[-2,-7],[-13,-29],[-30,22],[-81,99],[-14,9],[-19,6],[-20,-2],[-18,-7],[-18,-2],[-18,11],[-28,-36],[-13,19],[-5,45],[0,41],[11,59],[-1,23],[-6,24],[-7,11],[-9,7],[-84,116],[-14,40],[-4,38],[0,47],[7,42],[13,22]],[[735,6049],[50,53],[53,60],[73,44],[174,11],[93,38],[84,89]],[[1262,6344],[128,0],[26,-52],[59,12],[79,51],[10,-13],[7,-3],[4,4],[4,9],[5,7],[17,9],[6,4],[52,12],[11,57],[13,4],[10,8],[9,12],[11,8],[14,2],[35,-19],[13,-4],[25,3],[15,-2],[14,-12],[151,-14]],[[1437,8109],[-12,-78],[-32,-55],[0,-55],[36,-38],[69,0],[48,-50],[21,-104],[12,-93],[36,-44],[57,-16],[46,-96]],[[1718,7480],[-24,-31],[-27,-6],[7,-18],[5,-3],[8,-3],[17,-2],[6,-4],[12,-22],[10,-13],[16,-28],[2,-12],[-2,-40],[3,-12],[6,-7],[6,-5],[15,-17],[2,-9],[1,-12],[0,-16],[1,-39],[-1,-17],[-14,-75],[-3,-7],[-5,-5],[-13,-7],[-11,-12],[-26,-32],[-5,-11],[1,-18],[-12,-82],[-22,-70],[-25,-98],[-95,-59],[-80,8],[-68,-127],[-81,-110],[-60,-115]],[[735,6049],[1,2],[-22,-3],[-15,-14],[-12,-18],[-15,-15],[-19,-9],[-23,-5],[-21,5],[-11,21],[-9,0],[-10,-23],[-8,8],[-7,27],[-3,32],[1,32],[4,24],[14,51],[15,-7],[18,20],[20,28],[20,20],[-32,1],[-11,23],[2,34],[16,102],[11,25],[14,-11],[9,0],[2,27],[10,15],[12,7],[5,9],[17,81],[33,88],[19,10],[10,24],[2,29],[-7,25],[5,8],[13,27],[-27,-11],[-3,-14],[4,-20],[-3,-29],[-45,-42],[-5,-7],[-20,-21],[-23,-45],[-18,-49],[-16,-60],[-19,-39],[-38,-61],[-38,-45],[-18,-9],[-8,22],[3,28],[7,20],[12,13],[15,8],[-10,3],[-8,-1],[-8,-5],[-11,-10],[-18,24],[16,73],[11,31],[14,29],[9,10],[19,17],[9,11],[7,14],[4,13],[2,4],[-7,4],[-11,8],[-18,-31],[-60,-63],[-16,-31],[-16,-68],[-19,-21],[-13,36],[-17,22],[-13,25],[-3,43],[18,-1],[15,17],[13,24],[9,23],[6,24],[5,28],[1,23],[-2,12],[4,11],[2,8],[2,20],[-25,-25],[-8,-14],[-4,-18],[-6,-8],[-39,-23],[-10,0],[-1,20],[1,20],[3,18],[7,16],[-8,17],[-1,16],[6,15],[12,15],[-26,3],[-8,21],[4,28],[11,22],[17,15],[24,12],[25,8],[22,3],[16,11],[9,26],[6,26],[6,11],[22,9],[26,40],[21,15],[-10,10],[-7,11],[-10,30],[-3,-18],[-4,-16],[-7,-12],[-9,-5],[-8,-3],[-21,-11],[-4,-4],[-6,-37],[-14,-30],[-21,-15],[-24,13],[-6,-9],[-7,-4],[-10,-1],[-13,0],[8,35],[-8,22],[-12,5],[-7,-18],[-38,-68],[-42,11],[-26,104],[-33,-3],[23,-31],[4,-34],[-7,-32],[-10,-28],[-28,28],[-16,8],[-21,1],[45,-46],[16,-24],[4,-30],[-13,-33],[-26,-34],[-24,-23],[-11,-3],[-3,6],[-7,2],[-6,-2],[-3,-6],[2,-11],[6,-17],[1,-9],[0,-63],[6,-10],[13,-8],[12,-11],[6,-20],[-2,-10],[-16,-35],[-24,16],[-49,10],[-24,19],[-29,39],[-22,41],[-14,40],[-2,35],[9,34],[21,35],[10,7],[26,11],[6,7],[1,13],[5,13],[5,10],[6,8],[0,12],[-6,-1],[-1,3],[0,5],[-1,6],[38,53],[46,72],[129,333],[27,143],[20,69],[134,261],[24,36],[33,24],[55,8],[112,-18],[56,14],[120,87],[125,91],[17,18],[9,18],[16,44],[10,16],[15,10],[7,-1],[7,-6],[113,-10],[31,6],[14,-3],[12,-11],[11,-14],[13,-10],[22,-7],[19,1],[41,9],[21,1],[59,-13]],[[1938,4290],[-47,-37],[-37,-15],[-2,-4],[-10,-8],[-11,-3],[-5,9],[3,16],[7,5],[9,2],[9,8],[43,84],[1,0]],[[1898,4347],[40,-57]],[[2684,4327],[-56,-11],[-102,11],[-40,33],[-53,71],[-56,33],[-69,-44],[-73,-33],[-45,-5],[-41,-44]],[[2149,4338],[-9,0],[-30,-81]],[[2110,4257],[-9,97],[-8,132],[41,71],[32,82],[0,60],[-16,105],[16,136],[45,99],[64,71],[110,11],[40,0]],[[2425,5121],[57,-65],[28,-44],[12,-159],[-20,-60],[-28,-55],[16,-77],[53,-44],[56,-27],[61,-104],[20,-82],[4,-77]],[[6042,6331],[-33,-26],[-41,-66],[-101,-16],[-109,10],[-49,66],[-48,22],[-90,-44],[-97,-44],[-117,-158],[-73,-88],[-117,-17],[-73,-82],[-102,-44],[-20,-416],[-40,-192],[-37,-121]],[[4895,5115],[-48,-1],[-138,-1]],[[4709,5113],[-26,136],[17,161],[30,58],[9,98],[-1,237],[18,662],[-1,110],[-42,25],[-14,18],[3,12],[6,74],[-2,59],[-18,121],[11,20],[7,7],[10,12],[125,141],[89,0]],[[4930,7064],[10,-64],[299,-5],[29,-44],[44,-66],[77,-38],[77,27],[150,132],[109,104],[102,27],[74,16]],[[5901,7153],[12,-24],[-3,-15],[-21,-65],[-9,-19],[-8,-11],[-7,-4],[-12,-10],[-12,-18],[-10,-17],[-8,-8],[-8,-6],[-13,-7],[-4,-5],[-18,-27],[-6,-13],[-5,-13],[-9,-50],[-12,-42],[-9,-18],[-9,-10],[-15,-5],[-14,-7],[-18,-13],[-35,-33],[-7,-16],[-3,-16],[2,-18],[6,-17],[9,-17],[11,-16],[18,-16],[179,-75],[11,-12],[6,-5],[8,-2],[19,-3],[8,-3],[7,-6],[13,-23],[7,-6],[33,-22],[6,-7],[6,-10],[17,-42],[5,-8],[33,-42]],[[4078,7478],[9,0],[25,-10],[33,2]],[[4145,7470],[13,-72],[13,-35],[12,-22],[13,-9],[15,-6],[19,-12],[19,-19],[23,-31],[9,-27],[2,-31],[-7,-56],[-2,-40],[2,-39],[7,-61],[9,-30],[12,-21],[13,-8],[15,-4],[58,2],[15,3]],[[4405,6952],[37,-61],[8,-132],[-29,-87],[-28,-82],[-53,-33],[-117,-11],[-109,-33],[-97,-99],[-98,-131],[-32,-77],[-105,-22],[-49,-33],[-65,-71],[-75,-60]],[[3593,6020],[-17,10],[-7,15],[-3,37],[-10,10],[-13,7],[-25,10],[-10,5],[-16,12],[-7,15],[-2,19],[8,38],[-3,47],[-9,8],[-7,-1],[-11,-4],[-15,-13],[-63,-84],[-52,-35],[-54,-28]],[[3277,6088],[7,266],[36,104],[89,82],[77,88],[73,22],[53,115],[24,99],[61,60],[60,38],[49,38],[166,6],[-16,77],[-37,38],[-12,71],[33,66],[4,88],[28,54],[17,82]],[[3989,7482],[23,17],[16,6],[9,-4],[15,-18],[7,-5],[10,-1],[9,1]],[[6998,8881],[-89,-76],[-52,-98],[-120,-123],[-136,-96],[-10,-19],[-12,-17],[-11,-19],[78,-161],[48,-58],[67,-98],[53,-175],[-5,-305]],[[6809,7636],[-149,-7],[-139,143],[-106,26],[-86,-45],[-19,-78],[76,-236],[0,-115],[-43,-52],[-110,-6],[-125,0],[-43,13],[-22,49],[-69,48],[-63,-13],[-9,-78],[-1,-132]],[[4930,7064],[149,139],[86,81],[0,80],[-30,98],[-47,80]],[[5088,7542],[54,108],[21,27],[12,13],[14,10],[48,22],[8,9],[7,13],[16,19],[20,17],[16,7],[9,13],[9,28],[-6,13],[-79,51],[-11,11],[-10,12],[-4,7],[-4,8],[-2,10],[0,13],[4,19],[103,175],[6,81],[3,52],[2,63],[16,40],[0,78],[0,111],[-63,81],[-21,66],[-4,90]],[[5252,8809],[11,16],[14,35],[9,52],[9,6],[11,1],[11,7],[19,41],[6,10],[1,1],[44,42],[16,24],[17,78],[10,18],[18,5],[32,-1],[58,26],[27,1],[33,-12],[24,-23],[54,-113],[32,-31],[31,-20],[25,-26],[8,-51],[-3,-16],[-14,-32],[-2,-15],[7,-19],[36,-49],[55,-139],[39,-54],[55,3],[20,13],[14,17],[15,31],[12,34],[5,25],[10,18],[25,22],[27,19],[21,9],[-9,40],[15,23],[27,17],[27,26],[0,13],[-3,19],[0,18],[13,7],[5,7],[-12,37],[2,17],[24,12],[65,-7],[13,27],[9,15],[47,51],[17,16],[9,-10],[9,-6],[20,-9],[-1,26],[8,7],[12,1],[13,9],[6,17],[2,4],[10,-9],[4,-10],[3,-1],[8,22],[52,27],[10,-7],[7,-12],[8,-11],[45,-12],[47,-30],[26,-7],[33,2],[10,-2],[13,-8],[19,-21],[10,-9],[10,-4],[4,-1],[23,-2],[13,-4],[17,-11],[27,-29],[16,-10],[26,-11],[47,-33],[27,-6],[25,-9],[46,-50]],[[2110,4257],[-18,-35],[-25,-10],[-19,16],[-18,34],[-14,40],[-4,36],[-41,-23],[-33,-25]],[[1898,4347],[27,42],[16,49],[21,43],[3,28],[-2,105],[2,13],[6,8],[19,10],[3,12],[9,20],[20,13],[19,16],[8,32],[-28,-20],[-17,-6],[-13,11],[-59,94],[-5,20],[-5,-2],[-4,-2],[-9,-7],[15,-57],[4,-52],[-14,-41],[-42,-26],[-49,7],[-25,43],[-4,59],[14,53],[56,94],[22,60],[-5,60],[-7,8]],[[1701,5664],[85,0],[106,-29],[261,-75],[102,35],[55,109],[98,106],[111,38]],[[2519,5848],[105,13]],[[2624,5861],[0,-197],[20,-39],[8,-60],[-24,-55],[-33,-44],[0,-142],[-65,-33],[-40,-49],[-12,-49],[-53,-72]],[[6285,4416],[-5,-106],[20,-82],[25,-76],[12,-148],[-16,-170],[-57,-55],[-37,-66],[-23,-75]],[[6204,3638],[-12,-71],[-15,-57],[-5,-10],[-19,-23],[-75,-68],[-29,-22]],[[6049,3387],[-1,12],[-16,18],[-32,12],[-53,7],[-56,-3],[-49,20],[-9,66],[15,79],[24,57],[13,12],[15,10],[14,11],[11,22],[2,17],[-6,30],[0,16],[6,23],[8,20],[5,20],[-2,23],[-57,67],[-14,9],[-18,16],[-16,20],[-16,32],[-13,4],[-14,-1],[-13,3],[-9,-2],[-24,1],[-17,7],[13,16],[15,14],[-3,10],[-21,18],[-20,30],[-15,28],[-5,32],[7,45],[1,26],[-10,12],[-14,9],[-10,17],[-6,50],[-5,15],[-13,18],[-35,32],[-10,18],[-9,48],[-6,14],[-8,9],[-22,11],[-8,6],[-8,18],[-16,62],[-32,82],[-47,81],[-55,71],[-100,85],[-10,21],[-19,120],[-10,41],[-14,17],[-19,3],[-46,18],[-34,5],[-238,-2]],[[6042,6331],[18,-26],[8,-18],[3,-13],[1,-14],[-1,-18],[2,-12],[4,-11],[4,-7],[6,-5],[8,-2],[29,-4],[10,-5],[10,-6],[8,-9],[62,-91],[14,-11],[7,-5],[18,-27],[11,4],[13,2],[10,8],[6,12],[4,16],[14,-6],[39,-7],[26,0],[11,2],[51,16],[11,-5],[9,-12],[14,-14],[20,-7],[36,2],[18,-7],[-16,-20],[2,-17],[29,-32],[5,-13],[10,-36],[3,-8],[12,6],[2,13],[0,14],[5,6],[10,6],[31,32],[9,11],[4,32],[-1,30],[11,9],[6,-5],[4,-50],[2,-7],[3,-9],[4,-7],[2,-10],[-1,-12],[-11,-24],[-7,-11],[-7,-12],[-3,-11],[0,-20],[3,-11],[4,-10],[4,-6],[2,-10],[-4,-18],[-44,-118],[-9,-17],[-9,-6],[-9,-1],[-17,2],[-29,9],[-5,-4],[-3,-10],[1,-17],[2,-12],[0,-17],[-6,-54],[0,-19],[2,-13],[3,-9],[6,-29],[7,-17],[4,-7],[8,-15],[2,-9],[0,-12],[-3,-17],[-12,-29],[-8,-14],[-8,-9],[-12,-9],[-6,-7],[-29,-59],[-16,-15],[-4,-6],[-4,-7],[-4,-7],[-5,-7],[-6,-5],[-5,-4],[-7,-2],[-14,1],[-7,0],[-4,-9],[-3,-12],[1,-42],[-2,-16],[-2,-15],[-8,-19],[-6,-11],[-12,-13],[-6,-13],[-3,-8],[-1,-7],[0,-11],[2,-19],[12,-40],[2,-10],[4,-34],[2,-9],[8,-15],[5,-18],[7,-16],[2,-10],[-1,-12],[-4,-14],[-18,-41],[-5,-5],[-7,-3],[-18,-3],[-7,-3],[-6,-10],[-4,-18],[-2,-59],[2,-15],[2,-9],[4,-8],[3,-7],[1,-7],[-6,-6],[-15,-8],[-6,-6],[-11,-18],[-13,-11],[-6,-6],[-4,-6],[-4,-8],[-1,-12],[-1,-19],[11,-89],[0,-26],[-3,-46],[-11,-42]],[[3342,4489],[-2,-77],[-5,-16],[-11,-1],[-13,2],[-13,-6],[-6,-16],[-12,-60],[-3,-8],[-3,-5],[-4,-3],[-8,-3],[-10,-7],[-1,-7],[2,-8],[0,-10],[-46,-197],[-6,-12],[-9,-10],[-5,-7],[-3,-11],[-6,-12],[-6,2],[-7,6],[-6,2],[-23,-18],[-7,-9],[-11,-19],[0,-14],[6,-17],[-1,-14],[-19,-7],[-23,-16],[-32,-71],[-23,-26],[-28,-6],[-26,3],[-26,-3],[-30,-21],[-26,37],[-28,-27],[-22,-56],[-6,-53],[6,-18],[19,-30],[1,-15],[-5,-2],[-21,-3],[-8,-5],[-13,-25],[-11,-27],[-18,-57],[-18,-31],[-24,-21],[-52,-33],[-41,-41],[-21,-4],[-30,14],[-52,43],[-21,7],[-35,-8],[-59,-25],[-32,-31],[-15,12],[-12,16],[-6,20],[5,28],[3,1],[21,-1],[5,3],[-1,6],[-1,8],[1,7],[37,50],[25,57],[16,27],[23,17],[24,2],[44,-12],[24,10],[0,12],[-82,-5],[-38,6],[-17,18],[3,7],[12,15],[4,10],[0,23],[10,37],[7,38],[-30,-41],[-13,-45],[-7,-48],[-14,-53],[-33,19],[-14,12],[-9,20],[11,9],[3,17],[-4,48],[1,-1],[4,9],[3,13],[1,10],[-3,6],[-13,10],[-3,4],[-4,36],[-1,54],[9,34],[24,-25],[-4,40],[1,17],[3,18],[-9,0],[-8,-13],[-11,-8],[-15,-3],[-17,-1],[-10,-8],[-6,-19],[-14,-84],[-9,-17],[-31,20],[-14,6],[-5,11],[10,28],[-13,16],[-13,28],[-10,35],[-1,35],[9,34],[29,52],[9,39],[-10,0],[-32,-34],[-50,-5],[-46,19],[-20,38],[-9,55],[0,28],[14,29],[13,20],[10,24],[7,28],[2,34]],[[2684,4327],[57,38],[32,66],[53,55],[81,60],[77,27],[211,-5],[76,-49],[71,-30]],[[1980,6427],[279,-25],[264,-191],[17,-149],[-29,-70],[4,-69],[4,-75]],[[3037,9052],[19,-43],[7,-31],[1,-11],[1,-14],[-23,-197],[-6,-26],[-46,-113],[0,-12],[6,-10],[9,-4],[10,-4],[7,-2],[7,3],[7,9],[11,27],[8,10],[8,5],[9,0],[7,-4],[4,-7],[10,-20],[6,-20],[2,-14],[0,-12],[-3,-9],[-32,-82],[-12,-86],[-1,-45],[10,-76]],[[3063,8264],[1,-31],[1,-16],[2,-8],[4,-9],[5,-6],[13,-8],[5,-6],[7,-16],[6,-19],[2,-9],[3,-8],[8,-8],[2,-5],[3,-6],[4,-7],[5,-7],[5,-6],[6,-5],[48,-25],[6,-5],[3,-9],[1,-13],[-3,-20],[1,-12],[2,-10],[1,-10],[-4,-13],[-14,-15],[-10,-9],[-29,-16],[-12,-10],[-8,-12],[-54,-102],[-30,-29],[-78,-48],[-5,-5],[-32,-43],[-17,-18],[-6,-5],[-6,-5],[-7,-14],[-7,-20],[-20,-91],[-14,-38],[-26,-54],[-6,-18],[28,-160]],[[2847,7255],[-97,6],[-12,-8],[-52,-26],[-118,-38],[-69,-5],[-9,1],[-14,14],[-2,8],[-25,344],[-21,8],[-6,4],[-13,0],[-82,20],[-55,23],[-60,-3],[-7,7],[-11,3],[-15,2],[-117,-3],[-20,-5],[-20,-9],[-33,-22],[-15,-16],[-10,-12],[-8,-9],[-12,-6],[-22,-2],[-16,0],[-24,4],[-7,4],[-6,4],[-6,2],[-11,2],[-51,-11],[-63,-30],[-20,-26]],[[1437,8109],[25,13],[58,47],[11,11],[29,-20],[27,1],[23,18],[18,35],[3,18],[3,41],[-6,21],[4,10],[21,-1],[11,-11],[5,-37],[11,-9],[20,-7],[0,-12],[-3,-14],[8,-13],[28,-5],[34,8],[33,18],[21,21],[7,24],[7,79],[0,32],[-20,215],[4,76],[12,74],[-2,34],[-15,30],[-12,9],[-48,10],[-22,12],[-16,15],[-29,43],[-8,18],[-3,14],[-6,11],[-37,17],[-14,12],[-26,34],[-26,21],[-27,12],[-35,7],[-11,6],[6,20],[0,39],[3,19],[30,45],[82,50],[32,36],[26,20],[37,12],[23,-9],[-18,-41],[91,2],[40,19],[22,45]],[[1868,9304],[36,-95],[2,-15],[16,5],[83,53],[20,24],[10,8],[12,3],[33,-5],[6,18],[8,2],[17,-17],[51,-18],[-13,52],[3,17],[13,-13],[11,-18],[15,-21],[15,-7],[31,57],[15,2],[17,2],[27,-17],[9,-25],[-9,-17],[-11,-5],[-6,-8],[3,-28],[11,-23],[14,-13],[19,-16],[13,-19],[10,-29],[6,-28],[2,-28],[-14,-62],[-2,-30],[3,-28],[10,-25],[10,-18],[12,-18],[14,-11],[16,2],[0,13],[-7,22],[-1,18],[16,2],[18,-15],[5,-21],[-1,-25],[-2,-27],[3,-10],[12,-23],[27,-11],[44,-16],[261,3],[18,46],[32,30],[28,43],[88,5],[38,36],[30,51],[22,39]],[[7158,3118],[-15,-73],[-17,-60],[-60,-33],[-49,-39],[-44,-60],[-17,-115],[64,-349]],[[7020,2389],[-19,-7],[-7,-1],[-26,0],[-23,-4],[-21,-10],[-23,-16],[-53,-55],[-54,-43],[-29,-6],[-18,8],[-6,24],[14,86],[-9,26],[-20,16],[-77,26],[-23,0],[-23,-9],[-45,-60],[-23,-17],[-10,30],[-28,-14],[-14,-3],[-15,0],[-15,-2],[-12,5],[-11,8],[-37,15],[-9,1],[-41,-8],[-20,-10],[-17,-15],[-16,-20],[-13,-25],[-32,-89],[-17,-28],[-79,-105],[-48,-42],[-24,-13],[-15,4],[-38,34],[-20,11],[-45,3],[-38,-16],[-27,-38],[-10,-62],[-27,33],[14,59],[31,61],[25,39],[20,44],[19,158],[15,3],[3,14],[0,22],[3,24],[10,22],[21,26],[21,53],[21,39],[26,29],[28,7],[31,4],[25,23],[18,36],[11,39],[-58,104],[-7,18],[-21,19],[-55,99],[-4,5],[-9,8],[-5,8],[-1,6],[2,20],[0,8],[-13,51],[-2,18],[-2,133],[6,64],[21,108],[-1,4],[-1,11]],[[6204,3638],[48,-39],[7,-11],[7,-19],[-13,-114],[3,-12],[4,-7],[50,-50],[20,-8],[122,-193],[2,-11],[3,-8],[3,-7],[12,-6],[55,-18],[20,-13],[14,-14],[13,-35],[2,-6],[17,-2],[30,1],[108,21],[23,9],[18,15],[9,9],[12,14],[69,55],[13,7],[57,-9],[226,-69]],[[9339,5033],[-29,-15],[-1,-16],[4,-19],[5,-15],[4,-3],[8,-1],[8,-3],[4,-10],[4,-32],[2,-9],[-3,-5],[-6,-2],[-4,-7],[6,-16],[5,-3],[25,3],[7,-39],[-50,-474],[-2,-17],[1,-40],[5,-42],[6,-32],[10,-29],[79,-149],[22,-33]],[[8898,3995],[18,140],[-12,115],[-65,115],[-64,77],[-81,44],[-57,33],[-138,11],[-52,16],[-33,71],[-61,33],[-190,-16],[-134,-93],[-72,-77],[-49,-258],[-10,-128]],[[7898,4078],[-166,74],[-276,-15],[-108,142],[-5,-1],[-39,10],[-12,0],[-114,29],[-105,-8],[-81,79],[-83,132]],[[6909,4520],[43,37],[98,22],[48,88],[49,43],[4,99],[48,82],[53,55],[12,181],[-36,82],[0,285],[48,148],[21,279],[64,99],[146,203],[69,136],[49,83],[68,49]],[[7693,6491],[98,-49],[117,-33],[73,-44],[263,-27],[12,-192],[12,-104],[65,-99],[57,-27],[97,-28],[49,-49],[24,-110],[8,-87],[69,-93],[134,-28],[129,-33],[65,-38],[12,-93],[41,-126],[68,-99],[106,-49],[64,-38],[83,-12]],[[8238,3231],[-242,-94],[-123,16],[-42,145],[-23,151],[-65,15]],[[7743,3464],[22,194],[111,90],[22,330]],[[3593,6020],[21,-16],[23,-31],[-25,-38],[-5,-9],[-2,-13],[1,-30],[-2,-20],[-9,-37],[-13,-36],[-3,-20],[0,-25],[22,-116],[3,-9],[6,-8],[11,-5],[12,-2],[13,0],[13,3],[39,18],[14,3],[15,1],[13,-2],[37,-12],[20,-2],[9,1],[14,4],[4,1],[5,-2],[5,-4],[10,-20],[58,-147],[82,-141],[3,-19],[2,-27],[-1,-62],[-2,-27],[-3,-19],[-20,-60],[-5,-19],[1,-17],[16,-71],[30,-76],[0,-1]],[[4005,4908],[-43,-12],[-23,1],[-22,7],[-25,12],[-32,27],[-25,29],[-24,21],[-31,2],[-203,-58],[-61,-30],[-48,-38],[-8,-16],[-1,-12],[1,-12],[1,-19],[-19,-135],[1,-6],[5,-20],[1,-9],[-5,-5],[-7,2],[-6,0],[-3,-9],[0,-23],[-1,-6],[-5,-2],[-42,-34],[-12,-17],[-22,-52],[-4,-5]],[[2624,5861],[121,38],[150,61],[50,21]],[[2945,5981],[60,23],[19,67],[16,3],[17,6],[13,-6],[22,-22],[22,-16],[11,-10],[10,-10],[28,-23],[27,-9],[15,7],[11,14],[15,38],[19,21],[27,24]],[[7743,3464],[-20,-56],[-15,-120],[-80,-118],[-137,-103],[-32,-14],[-37,-7],[-24,-1],[-240,73]],[[6285,4416],[172,-29],[12,1],[14,3],[25,13],[11,9],[8,9],[14,21],[29,62],[34,56],[17,21],[4,3],[5,4],[16,7],[22,6],[67,5],[73,-4],[12,-3],[39,-23],[50,-57]],[[5027,8845],[10,-612],[-12,-71],[-126,-33],[-105,-44],[-36,-109],[-98,-99],[-239,-137],[-194,-148],[-69,-60],[-13,-62]],[[4078,7478],[-9,65],[-8,66],[-49,65],[-66,138],[-116,219],[-81,153],[-24,91]],[[3725,8275],[158,2],[40,-33],[81,-38],[45,-49],[69,-17],[93,-2],[35,37],[26,-5],[26,6],[14,19],[53,22],[48,33],[89,22],[154,5],[17,71],[32,143],[61,76],[36,137],[61,170],[3,143]],[[4866,9017],[83,-49],[24,-26],[54,-97]],[[3374,9430],[-1,-3],[-43,-38],[-62,-38],[-43,-37],[-19,-22],[-15,-12],[-15,-9],[-70,-32],[-18,-15],[-12,-16],[-36,-138],[-3,-18]],[[1868,9304],[0,1],[6,48],[10,36],[2,21],[-4,53],[1,23],[3,15],[23,75],[5,31],[-3,32],[-15,37],[-68,113],[-13,49],[-2,41],[10,120],[436,-21],[61,-22],[4,-2],[6,-3],[12,-11],[7,-3],[7,0],[4,5],[3,5],[7,3],[40,11],[12,1],[11,-4],[17,-14],[12,-3],[11,3],[23,8],[10,1],[8,-4],[13,-19],[6,-4],[5,1],[17,7],[12,0],[10,2],[9,-1],[10,-10],[13,20],[24,-16],[25,1],[23,5],[21,-4],[17,-25],[6,-34],[-1,-37],[-6,-32],[-20,-62],[-5,-30],[6,-35],[16,-29],[20,-15],[23,-3],[26,4],[24,-12],[23,2],[15,18],[4,35],[23,72],[49,1],[53,-36],[33,-39],[0,-7],[-3,-13],[3,-15],[14,-18],[13,-6],[40,-4],[22,-14],[6,-18],[8,-15],[27,-4],[87,6],[28,-4],[26,-15],[23,-23],[38,-56],[7,-18],[1,-13],[5,-7],[19,0],[2,0]],[[6809,7636],[95,-22],[44,-66],[69,-49],[20,-71],[25,-186],[12,-225],[32,-55],[53,-38],[48,-93],[61,-115],[69,-17],[397,6]],[[7734,6705],[-28,-77],[-9,-60],[-4,-77]],[[3989,7482],[-153,-49],[-26,-127],[-150,-67],[-68,-21],[-29,6],[-32,14],[-9,7],[-13,12],[-5,6],[-5,9],[-3,12],[-1,20],[2,25],[0,11],[-3,16],[-12,55],[-14,44],[-24,38]],[[3444,7493],[6,154],[20,82],[20,104],[41,50],[0,120],[-21,93],[-40,50],[8,125]],[[3478,8271],[247,4]],[[3444,7493],[-18,30],[-88,-3],[-34,14],[-25,-15],[-5,-10],[0,-9],[4,-18],[-1,-12],[-3,-11],[-7,-14],[-7,-8],[-7,-6],[-37,-12],[-19,-9],[-10,-13],[-14,-20],[-33,-81],[-3,-9],[-1,-11],[-2,-34],[-1,-10],[-5,-18],[-7,-17],[-2,-8],[-2,-20],[-2,-10],[-3,-8],[-1,-10],[-5,-17],[-4,-20],[-25,-57],[-10,-37],[-10,-62],[-1,-2],[-3,-1],[-5,-2],[-5,2],[-4,2],[-1,1],[-12,15],[-6,5],[-6,2],[-8,-2],[-3,-7],[0,-7],[1,-4],[0,-4],[-1,-4],[-3,-11],[-5,-6],[-7,-3],[-8,0],[-6,0]],[[2974,6922],[-46,154],[-56,85],[-25,94]],[[3063,8264],[415,7]],[[9453,1515],[-2,-45],[-14,-70],[-67,-137],[-3,-74],[10,-19],[30,-35],[7,-20],[-2,-18],[-8,-10],[-9,-7],[-6,-10],[-5,-29],[-5,-61],[-7,-27],[-43,-67],[-1,-7],[4,-18],[-1,-7],[-5,-8],[-16,-12],[-6,-7],[-27,-46],[-6,-21],[-1,-13],[5,-30],[-1,-13],[-39,-58],[-49,28],[-51,54],[-45,15],[-48,-19],[-25,-3],[-32,50],[-34,-24],[-61,-76],[-41,35],[-43,43],[-26,41],[-8,50],[7,68]],[[8779,908],[48,60],[29,77],[-29,99],[-24,98],[0,115],[36,110],[45,99],[20,87],[41,50],[12,65],[8,104]],[[8317,2020],[-41,-44],[-64,-169],[-29,-126],[-58,-54],[-22,-53]],[[8103,1574],[-23,-52],[-30,-57],[-31,-25],[-60,-64],[-25,-75],[-99,-119],[-47,-49],[-35,-104],[-24,11],[-28,-5]],[[7701,1035],[-18,30],[-12,31],[-35,61],[-31,76],[-13,76],[3,77],[18,79],[1,5],[10,17],[3,19],[-2,18],[-7,20],[-16,25],[-14,2],[-15,-15],[-16,-27],[2,21],[15,33],[3,19],[-25,132],[0,21],[-5,22],[-13,10],[-33,9],[-19,19],[-3,49],[9,52],[13,29],[7,8],[2,8],[-2,8],[-7,8],[-7,18],[-6,20],[-6,20],[-13,15],[15,21],[8,9],[9,6],[6,4],[16,13],[-7,5],[-9,14],[-24,23],[-67,38],[-18,32],[-11,11],[-9,-15],[-7,-12],[-10,-6],[-11,-2],[-11,2],[-23,-1],[-21,-6],[-12,5],[2,33],[13,23],[18,17],[11,21],[-8,39],[-17,24],[-27,17],[-27,0],[-16,-25],[-3,-38],[-1,-26],[-5,-5],[-19,27],[-14,32],[-6,33],[-8,85],[-8,32],[-14,23],[-23,5],[0,-23],[-6,-15],[-10,-14],[-9,-16],[0,-14],[5,-14],[-2,-12],[-34,-13],[-24,-17],[-12,-5],[-7,12],[-6,8],[-6,4]],[[3374,9430],[10,4],[39,29],[26,14],[23,6],[23,-2],[29,-9],[58,-28],[24,-23],[39,-78],[21,-15],[228,79],[50,37],[44,54],[25,16],[33,1],[81,-36],[29,-3],[18,4],[51,38],[15,14],[6,3],[9,0],[3,-2],[2,-3],[81,-46],[28,-8],[110,-6],[51,11],[104,52],[56,13],[116,12],[45,-6],[89,-42],[37,-2],[0,-36],[-26,-4],[-32,1],[-21,-15],[-6,-41],[9,-72],[0,-42],[-25,-72],[-38,-53],[-22,-54],[25,-73],[25,-30]],[[4709,5113],[-435,-3],[-7,-111],[-9,-17],[-253,-74]],[[4405,6952],[37,22],[60,14],[59,5],[45,11],[23,22],[38,50],[4,7],[58,84],[8,25],[86,64],[14,16],[7,22],[4,64],[5,22],[10,14],[16,6],[12,2],[9,3],[12,8],[8,12],[5,16],[8,8],[15,-11],[18,18],[16,-6],[18,-11],[22,-1],[15,18],[23,57],[28,29]],[[7734,6705],[40,60],[102,99],[117,54],[109,61],[126,115],[73,93],[48,181],[17,197],[28,164],[56,179],[33,57],[12,50],[15,45],[27,23],[28,49]],[[8565,8132],[18,-17],[30,-132],[5,-33],[11,-36],[15,-35],[15,-26],[73,-45],[37,-3],[34,36],[8,-23],[6,-38],[5,-34],[11,-26],[16,-9],[37,-1],[82,-33],[40,-26],[30,-33],[-24,-9],[-28,-17],[-16,-24],[11,-27],[25,-6],[24,5],[14,-8],[-8,-47],[-15,-24],[-25,-12],[-27,-2],[-24,4],[-52,16],[-13,-13],[-14,-41],[1,-7],[4,-9],[3,-11],[-2,-13],[-5,-4],[-13,-2],[-5,-2],[-11,-10],[-28,-16],[-10,-7],[-16,-32],[-8,-65],[-11,-28],[-12,-9],[-27,0],[-11,-6],[-13,-22],[0,-18],[3,-17],[-1,-21],[-14,-37],[-62,-84],[-12,-23],[-11,-30],[-5,-32],[6,-28],[20,-20],[21,9],[24,16],[26,4],[23,-5],[24,-1],[24,4],[22,9],[24,22],[13,28],[35,98],[23,0],[27,-12],[27,1],[14,5],[13,-2],[25,-12],[13,2],[26,20],[16,0],[30,-20],[28,-35],[21,-42],[8,-41],[2,-189],[-3,-34],[-8,-16],[-10,-14],[-28,-102],[-3,-37],[36,-19],[6,-32],[21,-361],[15,-48],[41,-75],[34,-85],[24,-17],[29,-1],[31,8],[21,12],[12,9],[8,-4],[14,-28],[4,-23],[2,-48],[7,-23],[27,-25],[35,-9],[73,-4],[26,-21],[20,-38],[14,-46],[9,-42],[2,-47],[-7,-42],[-29,-88],[-14,-20],[-10,-23],[-7,-24],[-4,-27],[-26,-45],[-31,-23],[-69,-34],[-25,-31],[-27,-50],[-15,-46],[13,-20],[17,-8],[-5,-18],[-16,-17]],[[8779,908],[-160,11],[-11,-36],[-35,-58],[-10,-38]],[[8563,787],[-109,21],[-102,60],[-37,109],[-36,148],[-59,148],[-14,139],[-30,89],[-73,73]],[[2945,5981],[-34,132],[-45,55],[-4,7],[-4,7],[-3,8],[-2,10],[1,21],[3,9],[7,7],[8,10],[8,14],[16,42],[1,11],[-5,5],[-5,2],[-3,6],[2,10],[7,14],[5,12],[4,15],[9,66],[-1,10],[-2,9],[-6,5],[-6,4],[-6,5],[-10,11],[-4,7],[-3,9],[-1,21],[-3,20],[-1,11],[0,20],[-1,10],[-3,7],[-4,7],[-3,6],[1,9],[3,10],[10,11],[5,9],[-1,14],[-5,20],[-1,11],[1,27],[-2,11],[0,9],[3,11],[10,12],[17,26],[5,43],[3,15],[0,10],[3,10],[8,10],[49,44],[8,4]],[[6998,8881],[25,-15],[13,0],[27,5],[10,1],[14,-8],[22,-23],[14,-11],[24,-10],[75,-3],[32,47],[17,31],[7,23],[-3,38],[2,31],[9,28],[21,29],[43,37],[151,68],[27,32],[22,20],[55,35],[23,4],[83,-19],[31,42],[7,24],[-4,43],[17,7],[13,29],[5,34],[0,23],[-8,9],[-23,7],[-10,8],[-6,17],[-1,16],[-4,14],[-37,27],[-24,31],[-20,31],[-7,18],[20,33],[63,21],[23,21],[9,2],[8,-1],[18,-14],[19,-10],[10,8],[10,12],[18,1],[103,-29],[57,-25],[161,-111],[46,-31],[24,-35],[15,-41],[2,-40],[-12,-40],[-18,-37],[-14,-38],[-2,-48],[4,-18],[13,-29],[5,-18],[9,-21],[17,1],[26,14],[21,-15],[17,-32],[11,-41],[-1,-40],[-21,-72],[-5,-40],[5,-34],[25,-28],[30,-2],[31,5],[28,-7],[24,-28],[6,-31],[-1,-34],[2,-39],[6,-20],[9,-18],[8,-18],[1,-21],[-68,-466],[25,3],[23,-14],[22,-18],[25,-11],[27,5],[22,14],[19,6]],[[5027,8845],[23,-26],[26,-24],[27,-18],[27,-13],[26,0],[44,24],[22,2],[21,7],[9,12]],[[8563,787],[-3,-63],[-2,-10],[-8,-19],[-2,-10],[1,-12],[6,-3],[6,-2],[3,-9],[2,-42],[-5,-36],[-14,-33],[-23,-33],[-37,-31],[-8,-9],[-5,-17],[-2,-37],[-5,-16],[-12,-14],[-22,-9],[-12,-11],[-25,-40],[-12,-26],[-6,-19],[2,-31],[10,-33],[7,-35],[-9,-36],[-15,-12],[-58,-30],[-18,-3],[-26,55],[-17,15],[-7,-43],[-15,8],[-6,-2],[-4,-29],[-6,-1],[-30,-12],[-11,-8],[-23,2],[-19,-13],[-17,-21],[-18,-18],[-18,-8],[-19,-4],[-8,-4],[-10,-4],[-16,-19],[13,63],[-5,24],[-26,18],[-11,11],[-1,32],[-7,7],[-30,-1],[-8,2],[-34,41],[-18,114],[-20,37],[-20,4],[-46,-17],[-21,5],[-9,12],[-9,37],[-9,15],[-12,10],[-9,4],[-19,4],[-23,-1],[-12,-3],[-6,-7],[-3,-19],[-7,-6],[-7,-4],[-21,-39],[-15,17],[-26,62],[-15,-30],[-40,-52],[-11,-7],[0,1],[-5,5],[15,17],[8,18],[4,22],[0,25],[6,23],[13,17],[27,28],[31,62],[11,39],[-5,25],[24,38],[10,21],[7,48],[8,28],[11,23],[15,15],[-29,45],[-2,61],[13,65],[15,47],[-16,29]]],"transform":{"scale":[0.0007419419926992687,0.0005483727897789705],"translate":[-15.081125454999949,7.190208232000117]}};
  Datamap.prototype.gmbTopo = '__GMB__';
  Datamap.prototype.gnbTopo = '__GNB__';
  Datamap.prototype.gnqTopo = '__GNQ__';
  Datamap.prototype.grcTopo = '__GRC__';
  Datamap.prototype.grdTopo = '__GRD__';
  Datamap.prototype.grlTopo = '__GRL__';
  Datamap.prototype.gtmTopo = '__GTM__';
  Datamap.prototype.gumTopo = '__GUM__';
  Datamap.prototype.guyTopo = '__GUY__';
  Datamap.prototype.hkgTopo = '__HKG__';
  Datamap.prototype.hmdTopo = '__HMD__';
  Datamap.prototype.hndTopo = '__HND__';
  Datamap.prototype.hrvTopo = '__HRV__';
  Datamap.prototype.htiTopo = '__HTI__';
  Datamap.prototype.hunTopo = '__HUN__';
  Datamap.prototype.idnTopo = '__IDN__';
  Datamap.prototype.imnTopo = '__IMN__';
  Datamap.prototype.indTopo = '__IND__';
  Datamap.prototype.ioaTopo = '__IOA__';
  Datamap.prototype.iotTopo = '__IOT__';
  Datamap.prototype.irlTopo = '__IRL__';
  Datamap.prototype.irnTopo = '__IRN__';
  Datamap.prototype.irqTopo = '__IRQ__';
  Datamap.prototype.islTopo = '__ISL__';
  Datamap.prototype.isrTopo = '__ISR__';
  Datamap.prototype.itaTopo = '__ITA__';
  Datamap.prototype.jamTopo = '__JAM__';
  Datamap.prototype.jeyTopo = '__JEY__';
  Datamap.prototype.jorTopo = '__JOR__';
  Datamap.prototype.jpnTopo = '__JPN__';
  Datamap.prototype.kabTopo = '__KAB__';
  Datamap.prototype.kasTopo = '__KAS__';
  Datamap.prototype.kazTopo = '__KAZ__';
  Datamap.prototype.kenTopo = '__KEN__';
  Datamap.prototype.kgzTopo = '__KGZ__';
  Datamap.prototype.khmTopo = '__KHM__';
  Datamap.prototype.kirTopo = '__KIR__';
  Datamap.prototype.knaTopo = '__KNA__';
  Datamap.prototype.korTopo = '__KOR__';
  Datamap.prototype.kosTopo = '__KOS__';
  Datamap.prototype.kwtTopo = '__KWT__';
  Datamap.prototype.laoTopo = '__LAO__';
  Datamap.prototype.lbnTopo = '__LBN__';
  Datamap.prototype.lbrTopo = '__LBR__';
  Datamap.prototype.lbyTopo = '__LBY__';
  Datamap.prototype.lcaTopo = '__LCA__';
  Datamap.prototype.lieTopo = '__LIE__';
  Datamap.prototype.lkaTopo = '__LKA__';
  Datamap.prototype.lsoTopo = '__LSO__';
  Datamap.prototype.ltuTopo = '__LTU__';
  Datamap.prototype.luxTopo = '__LUX__';
  Datamap.prototype.lvaTopo = '__LVA__';
  Datamap.prototype.macTopo = '__MAC__';
  Datamap.prototype.mafTopo = '__MAF__';
  Datamap.prototype.marTopo = '__MAR__';
  Datamap.prototype.mcoTopo = '__MCO__';
  Datamap.prototype.mdaTopo = '__MDA__';
  Datamap.prototype.mdgTopo = '__MDG__';
  Datamap.prototype.mdvTopo = '__MDV__';
  Datamap.prototype.mexTopo = '__MEX__';
  Datamap.prototype.mhlTopo = '__MHL__';
  Datamap.prototype.mkdTopo = '__MKD__';
  Datamap.prototype.mliTopo = '__MLI__';
  Datamap.prototype.mltTopo = '__MLT__';
  Datamap.prototype.mmrTopo = '__MMR__';
  Datamap.prototype.mneTopo = '__MNE__';
  Datamap.prototype.mngTopo = '__MNG__';
  Datamap.prototype.mnpTopo = '__MNP__';
  Datamap.prototype.mozTopo = '__MOZ__';
  Datamap.prototype.mrtTopo = '__MRT__';
  Datamap.prototype.msrTopo = '__MSR__';
  Datamap.prototype.musTopo = '__MUS__';
  Datamap.prototype.mwiTopo = '__MWI__';
  Datamap.prototype.mysTopo = '__MYS__';
  Datamap.prototype.namTopo = '__NAM__';
  Datamap.prototype.nclTopo = '__NCL__';
  Datamap.prototype.nerTopo = '__NER__';
  Datamap.prototype.nfkTopo = '__NFK__';
  Datamap.prototype.ngaTopo = '__NGA__';
  Datamap.prototype.nicTopo = '__NIC__';
  Datamap.prototype.niuTopo = '__NIU__';
  Datamap.prototype.nldTopo = '__NLD__';
  Datamap.prototype.nplTopo = '__NPL__';
  Datamap.prototype.nruTopo = '__NRU__';
  Datamap.prototype.nulTopo = '__NUL__';
  Datamap.prototype.nzlTopo = '__NZL__';
  Datamap.prototype.omnTopo = '__OMN__';
  Datamap.prototype.pakTopo = '__PAK__';
  Datamap.prototype.panTopo = '__PAN__';
  Datamap.prototype.pcnTopo = '__PCN__';
  Datamap.prototype.perTopo = '__PER__';
  Datamap.prototype.pgaTopo = '__PGA__';
  Datamap.prototype.phlTopo = '__PHL__';
  Datamap.prototype.plwTopo = '__PLW__';
  Datamap.prototype.pngTopo = '__PNG__';
  Datamap.prototype.polTopo = '__POL__';
  Datamap.prototype.priTopo = '__PRI__';
  Datamap.prototype.prkTopo = '__PRK__';
  Datamap.prototype.prtTopo = '__PRT__';
  Datamap.prototype.pryTopo = '__PRY__';
  Datamap.prototype.pyfTopo = '__PYF__';
  Datamap.prototype.qatTopo = '__QAT__';
  Datamap.prototype.rouTopo = '__ROU__';
  Datamap.prototype.rusTopo = '__RUS__';
  Datamap.prototype.rwaTopo = '__RWA__';
  Datamap.prototype.sahTopo = '__SAH__';
  Datamap.prototype.sauTopo = '__SAU__';
  Datamap.prototype.scrTopo = '__SCR__';
  Datamap.prototype.sdnTopo = '__SDN__';
  Datamap.prototype.sdsTopo = '__SDS__';
  Datamap.prototype.senTopo = '__SEN__';
  Datamap.prototype.serTopo = '__SER__';
  Datamap.prototype.sgpTopo = '__SGP__';
  Datamap.prototype.sgsTopo = '__SGS__';
  Datamap.prototype.shnTopo = '__SHN__';
  Datamap.prototype.slbTopo = '__SLB__';
  Datamap.prototype.sleTopo = '__SLE__';
  Datamap.prototype.slvTopo = '__SLV__';
  Datamap.prototype.smrTopo = '__SMR__';
  Datamap.prototype.solTopo = '__SOL__';
  Datamap.prototype.somTopo = '__SOM__';
  Datamap.prototype.spmTopo = '__SPM__';
  Datamap.prototype.srbTopo = '__SRB__';
  Datamap.prototype.stpTopo = '__STP__';
  Datamap.prototype.surTopo = '__SUR__';
  Datamap.prototype.svkTopo = '__SVK__';
  Datamap.prototype.svnTopo = '__SVN__';
  Datamap.prototype.sweTopo = '__SWE__';
  Datamap.prototype.swzTopo = '__SWZ__';
  Datamap.prototype.sxmTopo = '__SXM__';
  Datamap.prototype.sycTopo = '__SYC__';
  Datamap.prototype.syrTopo = '__SYR__';
  Datamap.prototype.tcaTopo = '__TCA__';
  Datamap.prototype.tcdTopo = '__TCD__';
  Datamap.prototype.tgoTopo = '__TGO__';
  Datamap.prototype.thaTopo = '__THA__';
  Datamap.prototype.tjkTopo = '__TJK__';
  Datamap.prototype.tkmTopo = '__TKM__';
  Datamap.prototype.tlsTopo = '__TLS__';
  Datamap.prototype.tonTopo = '__TON__';
  Datamap.prototype.ttoTopo = '__TTO__';
  Datamap.prototype.tunTopo = '__TUN__';
  Datamap.prototype.turTopo = '__TUR__';
  Datamap.prototype.tuvTopo = '__TUV__';
  Datamap.prototype.twnTopo = '__TWN__';
  Datamap.prototype.tzaTopo = '__TZA__';
  Datamap.prototype.ugaTopo = '__UGA__';
  Datamap.prototype.ukrTopo = '__UKR__';
  Datamap.prototype.umiTopo = '__UMI__';
  Datamap.prototype.uryTopo = '__URY__';
  Datamap.prototype.usaTopo = '__USA__';
  Datamap.prototype.usgTopo = '__USG__';
  Datamap.prototype.uzbTopo = '__UZB__';
  Datamap.prototype.vatTopo = '__VAT__';
  Datamap.prototype.vctTopo = '__VCT__';
  Datamap.prototype.venTopo = '__VEN__';
  Datamap.prototype.vgbTopo = '__VGB__';
  Datamap.prototype.virTopo = '__VIR__';
  Datamap.prototype.vnmTopo = '__VNM__';
  Datamap.prototype.vutTopo = '__VUT__';
  Datamap.prototype.wlfTopo = '__WLF__';
  Datamap.prototype.wsbTopo = '__WSB__';
  Datamap.prototype.wsmTopo = '__WSM__';
  Datamap.prototype.yemTopo = '__YEM__';
  Datamap.prototype.zafTopo = '__ZAF__';
  Datamap.prototype.zmbTopo = '__ZMB__';
  Datamap.prototype.zweTopo = '__ZWE__';

  /**************************************
                Utilities
  ***************************************/

  //convert lat/lng coords to X / Y coords
  Datamap.prototype.latLngToXY = function(lat, lng) {
     return this.projection([lng, lat]);
  };

  //add <g> layer to root SVG
  Datamap.prototype.addLayer = function( className, id, first ) {
    var layer;
    if ( first ) {
      layer = this.svg.insert('g', ':first-child')
    }
    else {
      layer = this.svg.append('g')
    }
    return layer.attr('id', id || '')
      .attr('class', className || '');
  };

  Datamap.prototype.updateChoropleth = function(data) {
    var svg = this.svg;
    for ( var subunit in data ) {
      if ( data.hasOwnProperty(subunit) ) {
        var color;
        var subunitData = data[subunit]
        if ( ! subunit ) {
          continue;
        }
        else if ( typeof subunitData === "string" ) {
          color = subunitData;
        }
        else if ( typeof subunitData.color === "string" ) {
          color = subunitData.color;
        }
        else {
          color = this.options.fills[ subunitData.fillKey ];
        }
        //if it's an object, overriding the previous data
        if ( subunitData === Object(subunitData) ) {
          this.options.data[subunit] = defaults(subunitData, this.options.data[subunit] || {});
          var geo = this.svg.select('.' + subunit).attr('data-info', JSON.stringify(this.options.data[subunit]));
        }
        svg
          .selectAll('.' + subunit)
          .transition()
            .style('fill', color);
      }
    }
  };

  Datamap.prototype.updatePopup = function (element, d, options) {
    var self = this;
    element.on('mousemove', null);
    element.on('mousemove', function() {
      var position = d3.mouse(self.options.element);
      d3.select(self.svg[0][0].parentNode).select('.datamaps-hoverover')
        .style('top', ( (position[1] + 30)) + "px")
        .html(function() {
          var data = JSON.parse(element.attr('data-info'));
          try {
            return options.popupTemplate(d, data);
          } catch (e) {
            return "";
          }
        })
        .style('left', ( position[0]) + "px");
    });

    d3.select(self.svg[0][0].parentNode).select('.datamaps-hoverover').style('display', 'block');
  };

  Datamap.prototype.addPlugin = function( name, pluginFn ) {
    var self = this;
    if ( typeof Datamap.prototype[name] === "undefined" ) {
      Datamap.prototype[name] = function(data, options, callback, createNewLayer) {
        var layer;
        if ( typeof createNewLayer === "undefined" ) {
          createNewLayer = false;
        }

        if ( typeof options === 'function' ) {
          callback = options;
          options = undefined;
        }

        options = defaults(options || {}, self.options[name + 'Config']);

        //add a single layer, reuse the old layer
        if ( !createNewLayer && this.options[name + 'Layer'] ) {
          layer = this.options[name + 'Layer'];
          options = options || this.options[name + 'Options'];
        }
        else {
          layer = this.addLayer(name);
          this.options[name + 'Layer'] = layer;
          this.options[name + 'Options'] = options;
        }
        pluginFn.apply(this, [layer, data, options]);
        if ( callback ) {
          callback(layer);
        }
      };
    }
  };

  // expose library
  if (typeof exports === 'object') {
    d3 = require('d3');
    topojson = require('topojson');
    module.exports = Datamap;
  }
  else if ( typeof define === "function" && define.amd ) {
    define( "datamaps", ["require", "d3", "topojson"], function(require) {
      d3 = require('d3');
      topojson = require('topojson');

      return Datamap;
    });
  }
  else {
    window.Datamap = window.Datamaps = Datamap;
  }

  if ( window.jQuery ) {
    window.jQuery.fn.datamaps = function(options, callback) {
      options = options || {};
      options.element = this[0];
      var datamap = new Datamap(options);
      if ( typeof callback === "function" ) {
        callback(datamap, options);
      }
      return this;
    };
  }
})();
