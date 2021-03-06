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
  Datamap.prototype.cubTopo = {"type":"Topology","objects":{"cub":{"type":"GeometryCollection","geometries":[{"type":"MultiPolygon","properties":{"name":null},"id":"-99","arcs":[[[0]],[[1]],[[2]],[[3]],[[4]],[[5]],[[6]],[[7]],[[8]],[[9]],[[10]],[[11]],[[12]],[[13]],[[14]],[[15]],[[16]],[[17]],[[18]],[[19]],[[20]],[[21]],[[22]],[[23]],[[24]],[[25]],[[26]],[[27]],[[28]],[[29]],[[30]],[[31]],[[32]],[[33]],[[34]],[[35]],[[36]]]},{"type":"Polygon","properties":{"name":"Cienfuegos"},"id":"CU.CF","arcs":[[37,38,39,40]]},{"type":"MultiPolygon","properties":{"name":"Isla de la Juventud"},"id":"CU.IJ","arcs":[[[41]],[[42]],[[43]],[[44]],[[45]]]},{"type":"MultiPolygon","properties":{"name":"Pinar del R??o"},"id":"CU.PR","arcs":[[[46]],[[47,48]]]},{"type":"MultiPolygon","properties":{"name":"Sancti Sp??ritus"},"id":"CU.SS","arcs":[[[49,50,-38,51,52]],[[53]],[[54]]]},{"type":"MultiPolygon","properties":{"name":"Ciego de ??vila"},"id":"CU.CA","arcs":[[[55]],[[56]],[[57]],[[58]],[[59]],[[60,61,62,-50]],[[63]],[[64]]]},{"type":"MultiPolygon","properties":{"name":"Camag??ey"},"id":"CU.CM","arcs":[[[65]],[[66]],[[67]],[[68]],[[69,70,-62,71]],[[72]],[[73]],[[74]],[[75]]]},{"type":"Polygon","properties":{"name":"Guant??namo"},"id":"CU.GU","arcs":[[76,77,78]]},{"type":"Polygon","properties":{"name":"Granma"},"id":"CU.GR","arcs":[[79,80,81,82]]},{"type":"MultiPolygon","properties":{"name":"Holgu??n"},"id":"CU.HO","arcs":[[[83]],[[-78,84,-80,85,86]]]},{"type":"Polygon","properties":{"name":"Las Tunas"},"id":"CU.LT","arcs":[[-86,-83,87,-70,88]]},{"type":"Polygon","properties":{"name":"Santiago de Cuba"},"id":"CU.SC","arcs":[[-77,89,-81,-85]]},{"type":"Polygon","properties":{"name":"Ciudad de la Habana"},"id":"CU.CH","arcs":[[90,91,92]]},{"type":"MultiPolygon","properties":{"name":"Artemisa"},"id":"CU.AR","arcs":[[[93]],[[94]],[[95,96,-48,97,-92]]]},{"type":"MultiPolygon","properties":{"name":"Matanzas"},"id":"CU.MA","arcs":[[[98]],[[99]],[[100]],[[101]],[[102,-40,103,104,105]],[[106]],[[107]]]},{"type":"MultiPolygon","properties":{"name":"Villa Clara"},"id":"CU.VC","arcs":[[[108]],[[109]],[[110]],[[111]],[[112]],[[-52,-41,-103,113]],[[114]],[[115]],[[116]],[[117]]]},{"type":"Polygon","properties":{"name":"Mayabeque"},"id":"CU.MQ","arcs":[[-105,118,-96,-91,119]]}]}},"arcs":[[[6528,2122],[-3,-1],[-1,32],[-8,14],[-4,1],[11,8],[13,-12],[1,-19],[-9,-23]],[[6076,2369],[-9,-4],[-7,12],[2,8],[4,5],[-6,12],[2,14],[3,19],[3,-1],[4,-24],[2,-16],[6,-2],[-4,-23]],[[6340,2485],[-7,-36],[-4,4],[-4,18],[2,9],[10,10],[3,-5]],[[9289,2501],[1,-40],[-3,0],[-1,7],[1,6],[-12,25],[-4,20],[18,-18]],[[5831,2740],[-3,-24],[1,1],[3,13],[4,-3],[1,-24],[-6,-38],[-1,-15],[3,-5],[1,-4],[-2,-11],[-6,2],[-13,69],[-2,12],[3,11],[4,1],[3,6],[4,11],[5,9],[1,-11]],[[5972,2695],[-3,-35],[-10,47],[-3,37],[-1,18],[6,-21],[5,-29],[4,-7],[2,-10]],[[5850,2892],[-5,-4],[-4,15],[-5,36],[1,6],[4,3],[4,0],[5,-23],[1,-17],[-1,-16]],[[5756,3720],[7,-24],[6,-26],[4,-25],[-2,1],[-4,10],[-2,23],[-3,-4],[-8,8],[-4,18],[-4,1],[-3,-1],[-3,9],[-2,8],[-1,5],[13,12],[2,-10],[4,-5]],[[5720,4625],[-8,-44],[-3,15],[5,4],[-2,26],[8,-1]],[[2337,5034],[-6,-12],[-19,29],[-9,6],[-3,14],[1,14],[7,4],[19,-6],[4,-7],[3,-7],[6,-8],[1,-10],[-4,-17]],[[2470,5104],[-8,-15],[-16,-14],[-20,-6],[-10,-10],[-1,5],[7,28],[5,11],[5,-2],[8,10],[9,-11],[-1,11],[3,12],[10,10],[5,2],[-1,-8],[-3,-2],[0,-4],[2,-8],[2,-8],[4,-1]],[[2210,5564],[1,-15],[1,2],[2,11],[5,13],[3,-5],[3,-23],[6,-4],[5,6],[6,-44],[2,-27],[-7,2],[-3,6],[-3,15],[-4,6],[-5,-13],[-8,-9],[-15,7],[9,15],[0,16],[-2,9],[-3,3],[-2,5],[2,16],[1,24],[-5,25],[-5,15],[0,5],[6,-7],[8,-23],[4,-5],[2,-5],[-2,-8],[-2,-13]],[[1632,5680],[0,-21],[-6,18],[-14,85],[-2,36],[2,12],[4,-14],[5,-29],[0,-8],[5,-33],[6,-46]],[[2876,5936],[-1,0],[0,8],[3,6],[3,15],[14,-1],[1,-12],[-8,0],[-12,-16]],[[2174,6012],[8,-24],[-1,-22],[-6,-17],[-5,6],[3,21],[4,13],[-5,-2],[-5,4],[-5,14],[-4,7],[2,7],[4,2],[10,-9]],[[6669,6015],[3,-11],[4,1],[3,4],[0,-19],[1,-8],[-4,-8],[-2,-4],[-19,32],[-1,22],[9,10],[3,-4],[3,-15]],[[2148,6015],[-1,-3],[-8,13],[-3,12],[6,-5],[6,-17]],[[1477,6161],[-75,-20],[-3,9],[43,38],[7,1],[14,-9],[10,-2],[7,-10],[-3,-7]],[[3234,6538],[7,-18],[3,5],[4,1],[4,-8],[3,-20],[4,-18],[18,-41],[6,-2],[2,8],[4,-32],[5,-9],[9,1],[10,10],[3,22],[-3,31],[-1,21],[3,0],[2,-9],[4,-4],[2,7],[-1,19],[3,9],[7,-36],[-1,-12],[0,-18],[4,-19],[5,-13],[5,-6],[4,-1],[4,9],[5,6],[7,-3],[1,-8],[-35,-35],[-3,7],[-3,9],[-5,10],[-7,-4],[-11,-13],[-10,1],[-48,114],[-11,12],[-3,14],[4,13]],[[6359,6816],[4,-2],[2,8],[0,7],[2,1],[3,4],[-2,-26],[-9,-10],[-9,16],[-8,19],[-2,25],[-4,17],[-1,10],[3,-2],[3,-14],[7,-14],[7,-14],[6,-14],[-2,-11]],[[6118,6962],[-6,-5],[5,10],[3,13],[-2,12],[-9,5],[-2,19],[2,21],[16,47],[18,7],[0,-16],[-10,-44],[-4,-32],[-11,-37]],[[6509,7112],[-3,-12],[-15,-43],[-6,-11],[0,15],[-9,29],[1,11],[3,12],[4,0],[7,-12],[7,0],[7,13],[4,-2]],[[6519,7207],[-7,-2],[-4,13],[0,23],[-2,11],[-13,30],[-3,13],[-8,27],[-2,12],[2,12],[6,-4],[14,-44],[5,-11],[8,-25],[6,-33],[-2,-22]],[[479,7525],[33,-31],[5,4],[4,7],[5,-3],[2,-21],[-4,-16],[-5,3],[-3,0],[-15,-22],[-11,9],[-11,17],[-10,8],[-4,8],[4,6],[-1,12],[2,12],[4,13],[4,10],[2,-5],[-1,-11]],[[6285,7692],[10,-36],[7,-39],[-1,-23],[-7,11],[-3,41],[-6,35],[-4,0],[-2,-17],[-3,-11],[-3,-34],[-3,-6],[-4,9],[-4,4],[-9,-5],[-5,7],[4,12],[7,9],[7,24],[5,24],[-1,11],[3,6],[12,-22]],[[2382,7714],[2,-21],[-2,0],[-8,13],[0,14],[3,11],[3,21],[0,25],[5,7],[0,-21],[-2,-29],[-1,-20]],[[5627,7793],[11,-3],[4,2],[3,-4],[6,-31],[8,-41],[-4,-10],[-9,7],[-5,21],[-3,6],[-2,-4],[-6,7],[-10,3],[-3,13],[2,19],[4,8],[4,7]],[[5525,7803],[-9,-14],[-2,7],[1,9],[-4,4],[1,28],[5,10],[9,3],[5,-36],[-6,-11]],[[5362,7903],[3,-16],[3,-11],[-12,-12],[-22,23],[4,27],[4,-20],[2,-6],[4,4],[-2,9],[-2,8],[5,3],[5,11],[6,-7],[2,-13]],[[5812,8066],[16,-62],[1,-20],[-6,-6],[-7,14],[1,11],[-2,8],[-5,1],[-9,25],[-6,2],[2,-7],[2,-10],[-5,-12],[-3,13],[5,34],[-2,5],[-3,-6],[-1,-13],[-2,0],[-2,6],[-5,-1],[-1,5],[2,16],[-1,11],[-5,8],[-2,7],[4,7],[15,-2],[19,-34]],[[5378,8117],[0,-11],[-6,1],[-2,-11],[-3,-14],[-3,5],[-1,20],[-5,5],[-8,-3],[-6,13],[0,19],[16,28],[9,3],[6,-8],[0,-13],[-1,-11],[1,-6],[1,-7],[2,-10]],[[5031,8268],[2,-9],[3,7],[2,-5],[1,-13],[-4,-12],[-3,-20],[2,-15],[-1,-12],[-10,3],[-6,21],[-1,22],[-3,14],[0,7],[0,10],[3,7],[4,21],[3,1],[1,8],[0,13],[2,6],[4,2],[2,-12],[-1,-20],[0,-24]],[[1097,8677],[0,-12],[-4,-12],[-4,-3],[0,10],[2,9],[-3,7],[-4,1],[-4,-3],[-6,-13],[-12,-1],[-7,-17],[-3,-22],[1,-12],[-4,-13],[-4,-8],[3,30],[10,47],[16,29],[11,-3],[12,-14]],[[4727,8913],[-11,-4],[0,8],[5,35],[6,6],[4,-14],[0,-23],[-4,-8]],[[4612,9096],[-4,-1],[-2,37],[-4,29],[-3,8],[2,9],[11,-1],[11,-19],[5,-23],[-1,-16],[-8,-9],[-2,-7],[-5,-7]],[[4565,9359],[13,-27],[2,-14],[-16,-62],[-9,-21],[-7,0],[-4,28],[0,40],[5,22],[1,-4],[10,-11],[2,7],[1,23],[-2,-1],[-5,-6],[-2,6],[2,16],[4,10],[5,-6]],[[3644,9889],[-4,0],[0,27],[8,44],[6,5],[2,-16],[-1,-15],[-11,-45]],[[4553,6139],[-33,-101],[-25,-59],[-7,-21],[-3,-14],[-1,-15],[-2,-120],[0,-1]],[[4482,5808],[-4,2],[-37,42],[-40,25],[-27,67],[-48,45],[-29,72],[-100,317],[-2,13],[0,15],[-1,16],[-4,16],[-4,2],[-11,-5],[-4,3],[-10,26],[-3,19],[1,34],[13,-7],[16,2],[16,14],[12,31],[-14,-11],[-5,-7],[3,25],[5,25],[4,24],[0,24],[-7,9],[-11,-9],[-9,1],[1,38],[-18,0],[-12,21],[-1,36],[14,44],[-23,40],[-8,25],[-7,35],[-7,-30],[-11,-11],[-14,5],[-13,17],[-5,-62],[9,-39],[27,-59],[14,-55],[12,-66],[6,-71],[-6,-68],[-39,44],[-74,-6]],[[4027,6476],[-7,32],[-2,18],[-2,33],[0,50],[-1,16],[-2,13],[-2,11],[-7,22],[-6,6],[-10,3],[-19,-7],[-14,-11],[-8,-1],[-5,5],[-6,17],[-4,6],[-9,0],[-7,-3],[-14,-2],[-4,-3],[-3,-7],[-2,-9],[-3,-7],[-3,0],[-7,8],[-13,23],[-32,38],[-29,49],[-10,45],[-11,32],[-39,80],[4,54],[-1,24],[-2,11],[0,14],[-1,6],[-2,3],[-4,1],[-15,-6],[-3,4],[-3,6],[-2,9],[-1,15],[-1,27],[1,21],[2,26],[5,40],[1,23],[0,15],[-3,19],[-1,12],[1,15],[3,18],[7,31],[1,15],[-1,12],[-3,6],[-3,3],[-3,0],[-3,-3],[-4,-6],[-4,-4],[-4,-1],[-2,1],[-1,6],[2,15],[14,69],[3,20],[6,47],[1,13],[-2,6],[-3,5],[-5,3],[-3,4],[0,5],[7,25],[11,23],[5,13],[3,10],[11,30],[5,10],[20,27],[11,20],[4,9],[8,16],[7,4],[10,-1],[47,-36],[7,-1],[21,5],[86,-27],[9,-9],[4,-1],[6,1],[20,28],[7,6],[5,4],[8,2],[4,3],[4,12],[6,24],[20,111],[3,12],[19,37]],[[4095,7894],[38,66],[12,38],[2,11],[4,9],[4,8],[8,-2],[5,-5],[22,-53],[23,12],[7,-4],[5,-16],[-1,-42],[-1,-27],[-5,-39],[-1,-13],[1,-8],[1,-6],[4,-4],[5,1],[11,10],[6,8],[10,20],[4,5],[5,1],[8,0],[5,0],[3,5],[1,8],[7,65],[4,19],[3,11],[3,3],[6,1],[12,-4],[6,1],[6,2],[3,6],[4,14],[2,6],[2,-9],[2,-19],[0,-51],[-2,-25],[-2,-14],[-3,-4],[-3,-3],[-3,-4],[-3,-8],[0,-15],[1,-25],[10,-70],[3,-15],[18,-29],[3,-30],[-4,-25],[-1,-21],[0,-17],[4,-21],[6,-10],[4,-11],[2,-13],[-3,-38],[-17,-56],[0,-31],[3,-19],[6,-23],[34,-96],[11,-17],[7,-4],[6,2],[14,12],[5,1],[4,-2],[2,-6],[2,-10],[2,-65],[3,-26],[5,-34],[8,-43],[12,-43],[23,-32],[-10,-93],[-4,-17],[-3,-24],[-1,-27],[3,-112],[0,-57],[0,-25],[10,-115],[5,-40],[6,-36],[17,-42],[5,-20],[2,-24],[2,-77],[-1,-43],[2,-28],[3,-25],[19,-42],[1,-6],[-1,-40]],[[2645,5062],[-20,-2],[-4,8],[11,12],[19,46],[10,8],[8,16],[7,22],[6,14],[26,30],[11,1],[11,15],[8,24],[3,20],[1,13],[2,4],[7,-22],[1,-52],[-16,-39],[-37,-40],[-27,-39],[-6,-5],[-5,-10],[-3,-12],[-13,-12]],[[2847,5269],[-29,-69],[-16,0],[-13,37],[-3,39],[3,-5],[10,-31],[15,5],[12,29],[4,17],[-4,1],[3,6],[28,36],[6,5],[12,8],[2,-15],[-14,-32],[-16,-31]],[[3265,5352],[-73,-144],[-29,-36],[-30,-9],[0,57],[8,15],[26,-12],[10,21],[9,43],[9,33],[10,-9],[12,-16],[14,10],[11,24],[4,23],[6,50],[13,35],[29,55],[8,8],[5,-12],[3,-18],[-3,-9],[-4,-7],[-38,-102]],[[1933,6125],[75,-55],[52,-43],[16,-21],[13,-32],[5,-45],[-4,-54],[-4,-36],[0,-33],[14,-47],[42,-99],[12,2],[9,-14],[6,-32],[5,-37],[5,-27],[0,-22],[-5,-34],[-10,-55],[-7,0],[5,-24],[1,-25],[-2,-25],[-4,-25],[21,-39],[11,-15],[13,-6],[0,-21],[-7,-35],[7,-37],[14,-30],[11,-17],[-10,-45],[-19,-60],[-21,-52],[-17,-22],[-17,-10],[-118,-175],[-71,-75],[-65,-21],[-11,12],[-9,27],[-60,3],[-9,9],[-18,23],[-11,8],[-33,-2],[-8,12],[-31,118],[-11,31],[-26,51],[-10,33],[-8,55],[-7,80],[-10,71],[-2,30],[6,0],[10,-30],[47,-100],[6,-41],[15,-34],[17,-25],[16,-9],[38,0],[3,12],[26,66],[15,29],[7,18],[3,23],[3,-2],[4,-18],[1,-29],[-8,-41],[6,-18],[17,90],[-5,34],[-27,13],[-11,31],[-80,410],[-11,115],[10,125],[39,90],[5,38],[8,30],[19,39],[21,32],[15,9],[-10,14],[-10,6],[-11,0],[-11,17],[-1,5],[6,7],[9,19],[121,-43]],[[2169,6091],[-25,-4],[-26,33],[-9,50],[21,16],[35,-46],[15,-35],[-11,-14]],[[1277,6220],[14,-14],[2,0],[5,-1],[-5,-5],[-18,-2],[-21,4],[-14,19],[10,15],[27,-16]],[[1854,9183],[-1,-16],[4,-105],[0,-13],[-3,-58],[23,-132],[4,-70],[-1,-26],[-17,-59],[-5,-12],[-7,-8],[-23,-8],[-4,-5],[-1,-5],[1,-6],[2,-4],[10,-14],[4,-10],[5,-18],[15,-77],[11,-44],[3,-7],[9,-14],[5,-11],[6,-24],[7,-36],[18,-124],[4,-23],[31,-100],[7,-32],[3,-17],[0,-11]],[[1964,8094],[-22,-22],[-56,-138],[-11,-21],[-21,-5],[-14,-13],[-62,-94],[-30,-59],[-25,-78],[-53,-295],[-22,-63],[-30,-11],[-30,26],[-13,3],[-13,-11],[-1,-7],[1,-41],[-2,-11],[-3,1],[-5,4],[-6,-7],[-4,4],[-4,1],[-1,-12],[0,-41],[0,-8],[-34,-63],[-11,-36],[-5,-25],[-1,-14],[1,-10],[-2,-13],[-6,-42],[-4,-18],[-8,-19],[-3,9],[-2,9],[0,-34],[-7,-20],[-5,30],[-8,9],[-8,-3],[-1,-48],[-15,-49],[-46,-11],[-48,17],[-40,49],[7,25],[-2,29],[-7,33],[-5,34],[-7,0],[-13,-77],[-23,-71],[-27,-50],[-25,-20],[2,10],[2,20],[2,11],[-103,-43],[-5,2],[-3,11],[-3,20],[-1,20],[1,9],[-10,-10],[-11,-41],[-14,-9],[-72,12],[-20,-12],[-38,-109],[-1,-22],[-7,-81],[-5,-28],[5,-19],[2,-18],[0,-43],[-10,26],[-11,-6],[-6,-32],[1,-47],[7,-39],[9,-14],[11,8],[12,27],[-6,-63],[-31,-70],[-8,-59],[3,-14],[7,-16],[3,-21],[-4,-28],[-9,-25],[-10,-23],[-10,-16],[-12,-6],[-17,12],[-35,55],[-20,12],[-21,-14],[-35,-64],[-17,-21],[-10,-3],[-19,5],[-9,-2],[-10,-13],[-21,-39],[-29,-25],[-109,-268],[-40,-64],[-39,-10],[-12,57],[37,229],[7,115],[-21,84],[-37,19],[-41,-18],[-73,-64],[-36,-50],[-93,-182],[-38,-27],[-40,2],[-39,35],[-12,30],[-8,42],[-2,48],[3,48],[3,19],[5,24],[6,19],[6,7],[5,-7],[3,-15],[2,-17],[3,-14],[7,-15],[7,-9],[8,-3],[9,2],[15,20],[11,33],[1,36],[-15,32],[15,25],[14,0],[9,-28],[0,-58],[76,119],[67,58],[79,104],[50,107],[7,11],[12,3],[11,-5],[6,-14],[-4,-22],[29,-38],[22,18],[21,37],[27,21],[13,-12],[16,-29],[13,-36],[5,-33],[5,-17],[25,54],[14,-7],[13,161],[-11,-37],[-10,-50],[-10,-27],[-13,33],[-5,55],[1,43],[-3,33],[-19,27],[9,63],[4,19],[-17,5],[-18,20],[-17,27],[-12,27],[-7,41],[-6,49],[-9,38],[-15,9],[9,26],[10,44],[9,54],[3,48],[2,67],[4,63],[13,128],[3,7],[13,74],[5,8],[10,3],[4,8],[4,19],[4,37],[2,14],[20,84],[13,45],[17,30],[0,21],[-19,0],[16,38],[40,22],[17,29],[11,44],[12,65],[4,63],[-11,37],[0,22],[10,-9],[9,-13],[13,29],[13,6],[10,-16],[8,-37],[8,109],[5,28],[9,11],[24,6],[5,14],[1,29],[3,50],[6,35],[12,-15],[13,-23],[9,15],[9,32],[10,27],[-7,33],[-9,21],[-13,8],[-15,-4],[4,22],[7,21],[8,14],[9,6],[15,-16],[8,-36],[6,-35],[9,-16],[13,6],[11,16],[56,119],[13,-38],[8,62],[14,27],[41,8],[15,9],[64,94],[32,34],[15,4],[10,-5],[12,-16],[10,-1],[6,7],[13,24],[7,9],[0,22],[-7,-9],[-6,-13],[-2,11],[1,6],[-1,4],[-5,1],[20,36],[20,18],[46,5],[4,18],[12,92],[6,9],[38,1],[-2,-21],[-2,-8],[-2,-11],[18,-10],[11,30],[7,45],[8,34],[14,17],[10,-13],[11,-24],[16,-20],[-7,-30],[11,10],[14,26],[8,23],[5,48],[14,63],[17,55],[17,24],[13,12],[47,89],[19,23],[17,2],[15,-15],[12,-32],[0,-18],[-8,-19],[-4,-34],[2,-41],[7,-37],[12,-15],[9,18],[8,29],[7,20],[-17,40],[1,38],[13,34],[21,25],[38,22],[8,10],[12,25],[5,4],[10,-5],[12,-25],[10,-9],[5,8],[6,16],[6,10],[9,-15],[6,-30],[-6,-9],[-9,-4],[-4,-16],[4,-30],[5,-10],[8,10],[8,30],[-2,-58],[16,-3],[19,35]],[[5567,7478],[-4,-41],[-2,-89],[-7,-21],[-8,-21],[-6,-34],[-9,-60],[-8,-93],[-10,-55],[-13,-68],[-12,-63],[-8,-68],[-12,-34],[1,-80],[-13,-93],[1,-60],[15,-38],[28,-68],[20,-29],[12,-17],[4,-51],[0,-68],[-13,-72],[-16,-72],[-6,-34],[-13,9],[-15,4],[-11,-21],[-5,-59],[-11,-64],[-13,-76],[-10,-64],[-9,-114],[-20,-127],[-7,-46],[15,-68],[7,-47],[-5,-80],[-5,-51],[8,-34],[26,0],[27,34],[10,38],[0,47],[14,59],[23,-30],[20,-42],[5,-38],[-2,-76],[2,-89],[5,-61]],[[5537,5153],[-10,-11],[-6,21],[-16,-26],[-17,-11],[-40,-3],[-18,-9],[-49,-70],[-38,-30],[-36,-9],[-38,10],[-74,63],[-15,28],[-17,46],[-13,6],[-48,-17],[-18,1],[-21,18],[-9,16],[-8,25],[-14,69],[-8,26],[-4,-25],[-3,-29],[-7,-19],[-9,-5],[-9,15],[-9,23],[-5,9],[-40,32],[-5,7],[-8,18],[-5,27],[-5,33],[-6,28],[-12,11],[-65,5],[-63,53],[-23,-1],[-11,-47],[-6,-51],[-16,7],[-20,32],[-21,22],[0,20],[19,22],[0,18],[-14,10],[-3,32],[1,41],[-3,36],[-16,-34],[-16,-15],[-18,0],[-20,9],[-19,20],[-11,8],[-8,-6],[-3,-21],[6,-19],[9,-15],[8,-5],[-12,-36],[-17,24],[-15,53],[-7,47],[-2,62],[-7,55],[-11,39],[-16,15],[-15,7]],[[4553,6139],[33,45],[17,41],[7,5],[13,-3],[5,9],[2,15],[2,24],[4,23],[9,34],[9,12],[9,4],[54,-38],[7,-8],[4,-10],[13,-21],[28,-6],[23,57],[18,88],[15,89],[5,54],[1,38],[-4,16],[-4,6],[-5,1],[-5,-2],[-4,3],[-3,8],[-3,19],[-4,33],[-14,77],[-6,50],[0,31],[2,19],[10,27],[31,-2],[10,-26],[4,-19],[4,-20],[10,-25],[10,-7],[8,1],[6,11],[14,26],[12,4],[8,-2],[38,-34],[19,18],[13,-16],[8,-16],[13,-16],[13,5],[10,13],[8,22],[8,31],[7,40],[5,37],[5,52],[16,50],[12,17],[16,18],[11,2],[9,-6],[23,-35],[24,-12],[3,53],[-1,46],[2,35],[4,36],[-1,21],[-26,106],[-31,181],[-8,37],[-11,24],[17,93],[0,67],[1,3]],[[5115,7692],[69,-172],[27,-38],[25,12],[9,-28],[17,-36],[17,-17],[8,31],[10,4],[48,-31],[17,-1],[10,21],[2,19],[3,13],[11,3],[10,-6],[14,-26],[11,-6],[20,9],[34,38],[18,13],[21,1],[51,-17]],[[5309,8134],[-6,0],[0,19],[-3,17],[-8,9],[8,9],[36,5],[-12,-30],[-15,-29]],[[5536,8206],[-22,-33],[-5,40],[-13,15],[-56,11],[-9,11],[-12,22],[16,6],[87,-16],[14,-24],[0,-32]],[[5730,2612],[-16,-40],[-22,23],[-21,51],[-11,46],[18,11],[20,-24],[32,-67]],[[5568,2888],[0,-38],[46,5],[17,-29],[10,-76],[-7,0],[-15,34],[-65,22],[-21,63],[11,50],[9,24],[9,-2],[4,-23],[2,-30]],[[5516,2957],[-6,-16],[-2,19],[-4,11],[-22,17],[-46,90],[-10,33],[12,5],[17,-3],[15,-13],[7,-20],[10,-13],[22,-8],[17,-17],[-2,-43],[-8,-42]],[[5331,3433],[42,-74],[35,-104],[24,-85],[-5,2],[-14,-2],[2,-16],[4,-43],[-26,39],[-18,47],[-32,110],[0,22],[17,-18],[11,-34],[11,-37],[12,-32],[-12,58],[-21,76],[-23,59],[-20,8],[0,-22],[3,-10],[1,-6],[0,-7],[3,-15],[-11,9],[-9,16],[-7,22],[-6,30],[39,7]],[[5193,3739],[36,-112],[-14,-18],[-15,-1],[-15,13],[-13,28],[7,-2],[19,2],[-4,31],[-7,20],[-8,4],[-7,-16],[-24,52],[-8,28],[30,12],[23,-41]],[[5567,7478],[91,-31],[43,7],[5,-29],[18,-169],[3,7],[9,12],[9,-37],[9,23],[14,92],[-12,49],[-6,19],[-8,13],[0,20],[28,-11],[29,-87],[101,-104],[13,-27],[8,-37],[18,-46],[22,-42],[16,-25],[22,-19],[56,-28],[17,7],[-1,-19],[1,-41],[0,-19],[9,14],[4,8],[6,-29],[9,-56],[6,-26],[9,-18],[16,-25],[7,-18],[8,-12],[9,0],[9,3],[9,-2],[32,-38],[14,-11],[46,-10],[8,-13],[2,-28],[1,-33],[4,-25],[8,-6],[24,1],[-3,-5],[-3,-11],[9,-26],[12,-13],[12,-8],[10,-10]],[[6349,6559],[-2,-26],[8,-15],[2,-10],[2,-16],[0,-17],[-1,-16],[-3,-31],[-4,-14],[-5,-11],[-12,-13],[-11,-17],[-17,-41],[-22,-69],[-10,-22],[-8,-12],[-11,-7],[-15,-1],[-15,-14],[-5,-2],[-7,-1],[-6,-9],[-5,-9],[-11,-84],[6,-51],[3,-10],[46,-112],[15,-48],[19,-92],[-12,-25],[-13,-20],[-3,-8],[-1,-8],[0,-12],[4,-60],[0,-9],[-5,-6],[-36,-14],[-13,1],[-10,13],[-14,57],[-3,16],[-5,17],[-3,15],[-1,14],[-1,17],[0,10],[-2,6],[-3,3],[-4,1],[-7,-8],[-10,-17],[-14,-40],[-5,-24],[-3,-16],[3,-55],[5,-46],[0,-18],[1,-23],[-1,-18],[-2,-11],[-2,-4],[-5,-2],[-3,-5],[-6,-28],[-4,-16],[-11,-49],[1,-20],[3,-14],[4,-8],[2,-6],[-3,-7],[-6,-4],[-18,-2],[-12,7],[-22,4],[-5,3],[-8,-12],[-9,-72],[-11,-85],[-2,-13],[-5,-16],[-4,0],[-3,3],[-1,7],[-2,8],[-5,17],[-2,5],[-2,2],[-16,-15],[-21,-28],[-10,-19],[-11,-29],[-3,-11],[-7,-17],[-48,-89],[-34,-44]],[[5831,4962],[-7,21],[-15,30],[-14,15],[-49,23],[-12,11],[-4,21],[6,40],[10,-17],[12,-6],[26,2],[3,13],[-4,30],[-12,46],[-7,18],[-9,20],[-19,32],[-14,12],[-6,-3],[-6,-12],[-12,-15],[-43,-29],[-43,-13],[-4,-5],[-6,-13],[-4,-16],[-1,-15],[-4,-10],[-7,-3],[-28,5],[-11,7],[-10,2]],[[5751,7893],[-7,-32],[-3,-18],[-17,-17],[-8,1],[-2,19],[0,47],[6,6],[13,-11],[8,4],[1,21],[2,8],[7,-9],[0,-19]],[[6099,7897],[10,-42],[-3,4],[-9,14],[5,-39],[61,-185],[4,-26],[-3,-29],[-7,1],[-8,12],[-7,6],[-115,-77],[0,17],[2,9],[2,8],[2,11],[1,23],[2,11],[7,-2],[6,-4],[4,4],[0,27],[-3,28],[-7,14],[-13,-19],[-18,-23],[-25,-10],[-42,3],[-21,19],[-25,51],[-38,21],[-27,25],[-16,32],[13,32],[-14,46],[-19,-41],[-15,-24],[-2,99],[3,-8],[9,-12],[3,58],[8,6],[10,-14],[7,0],[10,5],[41,-25],[23,-42],[26,-6],[26,6],[24,-6],[22,4],[15,35],[12,39],[17,19],[21,-6],[22,-18],[19,-31]],[[6069,2136],[16,-22],[2,45],[9,10],[11,-18],[9,-37],[7,-57],[-2,-40],[-10,-10],[-17,37],[-11,17],[-14,12],[-11,17],[-13,79],[-16,35],[-33,49],[13,31],[15,-10],[13,-31],[10,-30],[14,-54],[8,-23]],[[5889,2414],[1,-27],[-7,-14],[-8,-2],[-6,7],[-5,14],[-5,10],[-2,12],[4,9],[5,-7],[22,-13],[-1,10],[0,5],[0,7],[2,-11]],[[5841,2401],[-14,-3],[-9,18],[1,16],[3,3],[7,-5],[13,-13],[-1,-16]],[[5982,2622],[2,-10],[10,16],[9,1],[18,-17],[-12,-41],[-43,-31],[-15,-29],[-15,24],[-16,14],[11,25],[33,21],[13,35],[5,-5],[0,-3]],[[7400,4741],[-3,-6],[-7,-21],[-15,-18],[-26,-60],[-12,-16],[-25,-19],[-16,-2],[-22,15],[-18,7],[-15,-133],[-8,-27],[-5,-38],[-1,-27],[4,-48],[5,-17],[4,-9],[35,1],[6,-4],[1,-10],[-1,-11],[-52,-142],[-5,-34],[-1,-20],[4,-188],[-33,-165],[-6,-21],[-12,-31],[-6,-5],[-27,-17],[-5,-6],[-9,-16],[-10,-16],[-13,-53],[-27,-189],[-8,-11],[-7,6],[-6,8],[-9,27],[-4,9],[-5,3],[-5,-8],[-6,-15],[-10,-61],[-9,29],[-3,19],[0,9],[1,8],[6,28],[0,8],[0,6],[-2,8],[-4,4],[-8,4],[-16,4],[-16,12],[-2,6],[-1,7],[1,18],[-1,13],[-2,13],[-8,37],[-3,9],[-4,15],[-1,8],[-1,7],[-1,6],[-1,0],[-3,-7],[-6,-22],[-14,-20],[-3,-7],[-2,-9],[0,-17],[1,-13],[2,-22],[0,-4],[-4,-25],[-1,-21],[-3,-20],[-6,-23],[-17,-10],[-12,-3],[-12,2],[-2,0],[-3,-10],[-2,-42],[-14,-6],[-18,-16],[-10,-15],[-5,-40],[0,-36],[-1,-9],[-3,-8],[-7,3],[-5,3],[-11,2],[-5,3],[-3,9],[-5,18],[-4,18],[-5,21],[-5,6],[-6,-3],[-7,-15],[0,-23],[2,-18],[0,-19],[0,-17],[-4,-47],[-1,-22],[0,-21],[0,-17],[-7,-15],[-14,-17],[-56,-32],[-22,4],[-48,-200],[-13,-237],[0,-1]],[[6580,2611],[-9,8],[-34,11],[-20,-14],[-13,-32],[-12,-33],[-14,-20],[-60,-5],[-32,24],[-10,62],[-7,0],[-5,-41],[-31,106],[-11,15],[-20,12],[-11,31],[-16,94],[-10,33],[-17,46],[-19,35],[-15,-2],[-9,-16],[-4,15],[0,27],[-2,21],[-11,39],[-6,28],[-3,25],[-6,42],[-66,140],[-12,11],[-33,-5],[-11,19],[-10,44],[-14,78],[-12,33],[-34,77],[-8,3],[-6,27],[-6,99],[-3,189],[-7,82],[-14,82],[-10,82],[6,84],[8,-30],[10,-4],[9,20],[4,42],[-7,29],[-30,30],[-7,21],[-2,51],[-9,94],[-1,55],[-4,50],[-18,88],[-4,42],[-5,98],[-15,92],[-18,80],[-13,37]],[[6349,6559],[1,-2],[14,14],[9,-28],[12,-76],[18,-49],[5,-20],[6,-53],[16,-86],[8,-28],[11,-12],[11,-22],[42,-128],[4,-24],[15,-85],[7,-21],[11,5],[5,29],[4,218],[6,28],[10,13],[25,-12],[14,-1],[-8,-30],[-29,-15],[-7,-25],[2,-52],[3,-39],[6,-31],[8,-28],[11,-29],[8,-11],[23,0],[10,-19],[7,-45],[4,-50],[1,-35],[3,-35],[9,-25],[12,-18],[26,-24],[5,-3],[14,19],[8,-12],[7,-20],[8,-11],[56,25],[17,-5],[15,-21],[41,-78],[1,40],[7,20],[24,19],[1,-28],[6,-15],[8,-3],[11,5],[-2,11],[-3,21],[-2,9],[13,-12],[13,-30],[19,-59],[-14,-5],[-9,16],[-6,2],[-3,-51],[2,-53],[14,-91],[3,-46],[5,-30],[12,-11],[28,-11],[22,-34],[6,-5],[9,6],[4,13],[3,14],[3,6],[-1,16],[-5,35],[-9,35],[-24,32],[-10,39],[-11,85],[-6,68],[-4,20],[-6,11],[-19,22],[-3,9],[-2,30],[-4,40],[-10,21],[-16,-24],[-13,42],[-13,-34],[-13,24],[-25,91],[-34,87],[-10,45],[-6,5],[-13,-18],[8,50],[14,52],[17,34],[15,-7],[32,-79],[3,-24],[6,-74],[3,-15],[99,-209],[18,-28],[48,-43],[13,-27],[13,-40],[31,-75],[13,-13],[54,-121],[14,-21],[13,-12],[13,0],[17,13],[-7,-55],[5,-35],[9,-25],[5,-26],[-3,-58],[-22,-70],[-6,-51],[-8,16],[-16,53],[-5,11],[-59,81],[-15,13],[-18,5],[1,-5],[-2,-11],[-3,-10],[-2,-3],[-2,-10],[-5,4],[-5,10],[-8,11],[-10,27],[-8,9],[-22,-5],[-7,-29],[3,-32],[10,-15],[11,-8],[8,-17],[7,-20],[9,-15],[9,-6],[26,6],[18,-22],[5,-48],[-7,-49],[-16,-22],[-3,-24],[6,-52],[8,-50],[4,-20],[11,-6],[20,10],[15,-5],[-7,-51],[9,7],[23,34],[1,4],[8,14],[9,4],[8,44],[12,154],[23,131],[7,28],[18,10],[17,-31],[99,-358],[15,-39],[2,-4]],[[6528,6539],[15,-1],[10,15],[9,23],[10,21],[16,13],[13,-2],[11,-14],[11,-17],[3,-12],[9,-39],[4,-9],[8,6],[13,28],[7,6],[10,-13],[30,-57],[11,-11],[12,19],[7,25],[9,2],[16,-46],[7,-36],[2,-36],[-2,-39],[-20,-131],[3,-28],[10,-48],[-14,-1],[-11,-13],[-19,-44],[4,52],[-6,24],[-10,-9],[-7,-47],[2,-9],[4,-19],[1,-21],[-7,-12],[-6,4],[-3,13],[-3,18],[-11,46],[-5,27],[-6,27],[-11,25],[-13,-18],[-20,12],[-20,28],[-13,28],[-2,25],[4,31],[5,33],[3,30],[-5,21],[-10,14],[-17,15],[4,-25],[2,-25],[-1,-25],[-5,-25],[-7,60],[-5,0],[-10,-41],[-15,-12],[-14,17],[-6,47],[-8,35],[-16,37],[-12,49],[5,67],[-7,19],[18,-1],[13,-24],[11,-31],[15,-21]],[[6648,6837],[-22,-2],[-23,72],[-37,168],[7,39],[-11,44],[-16,38],[-11,20],[22,11],[20,-48],[15,-72],[12,-125],[14,-49],[17,-45],[13,-51]],[[6436,7007],[8,-11],[23,10],[11,-10],[6,-26],[4,-30],[7,-22],[14,-4],[-12,22],[3,18],[4,16],[5,14],[6,12],[6,-36],[10,-17],[9,4],[7,31],[7,-50],[-10,-22],[-17,-11],[-12,-18],[10,-9],[8,-17],[7,-19],[7,-15],[9,-10],[19,-11],[10,-19],[-21,-122],[-14,-49],[-10,31],[-4,-6],[-11,-9],[-4,-6],[-3,23],[-2,12],[-7,25],[-4,59],[-20,20],[-15,-24],[8,-73],[-12,16],[-33,57],[-6,16],[-7,43],[-29,61],[-9,45],[15,17],[12,37],[8,48],[3,47],[1,106],[-3,33],[-10,50],[14,-7],[14,-14],[11,-26],[5,-41],[-5,-26],[-10,-30],[-7,-36],[6,-47]],[[6242,7590],[7,-37],[18,30],[24,-17],[23,-45],[17,-49],[9,-36],[8,-43],[6,-45],[4,-97],[4,-30],[20,-65],[8,0],[13,-15],[11,-5],[-15,-44],[-22,18],[-46,86],[-20,12],[-22,0],[-23,14],[-23,53],[19,37],[-9,48],[-18,28],[-11,-24],[-1,-27],[-4,-30],[-6,-19],[-8,7],[-6,31],[-1,37],[5,22],[8,-10],[0,50],[-14,58],[-22,28],[-21,-39],[-5,15],[-4,4],[-4,-1],[-6,4],[13,47],[18,14],[33,-2],[30,35],[13,2]],[[8844,159],[-56,234],[-20,55],[-6,20],[-12,61],[-4,43],[-1,108],[12,16],[3,0],[4,7],[11,41],[22,32],[3,10],[1,16],[-1,26],[-2,13],[-6,10],[-4,1],[-2,2],[-3,6],[-3,33],[1,77],[-9,19],[-2,6],[0,4],[-5,39],[-2,12],[-5,42],[-5,70],[-1,18],[1,14],[4,23],[3,11],[2,7],[3,7],[2,6],[2,16],[1,94],[-2,48],[-10,59],[-3,25],[1,8],[5,11],[11,14],[11,20],[10,26],[10,42],[3,23],[2,19],[0,14],[0,11],[2,9],[2,4],[5,-3],[3,-5],[9,-9],[5,3],[3,9],[5,24],[8,21]],[[8850,1731],[11,8],[18,37],[7,4],[6,-5],[4,-11],[8,-18],[5,-15],[4,-16],[1,-4],[6,-17],[8,-15],[4,-5],[5,1],[16,25],[4,1],[3,-7],[2,-9],[13,-30],[9,44],[2,88],[3,16],[12,33],[21,37],[6,4],[5,-3],[8,-19],[4,-3],[6,3],[17,27],[21,24],[6,5],[5,-2],[6,-14],[26,-83],[21,-50],[30,-7],[25,-52],[6,-4],[4,3],[0,11],[0,8],[0,8],[-1,8],[-3,13],[-3,13],[-6,13],[-7,3],[-1,1],[0,4],[0,9],[2,21],[0,8],[-1,8],[-4,23],[2,5],[5,5],[25,11],[9,-3],[9,-14],[31,-67],[34,-46],[10,-18],[9,-21],[13,-17],[6,-4],[3,5],[2,9],[1,17],[1,12],[1,8],[3,9],[2,5],[38,45],[-8,78],[1,57],[5,46],[5,27],[7,29],[5,17],[7,18],[3,5],[4,1],[7,-1],[3,2],[3,6],[9,21],[12,18]],[[9461,2118],[5,-11],[33,-33],[4,-38],[17,-43],[42,-79],[8,-26],[10,-61],[29,-121],[7,-13],[3,-18],[45,-141],[9,-13],[12,-5],[21,-17],[24,-45],[21,-59],[8,-61],[6,0],[13,41],[19,0],[20,-15],[18,-4],[15,20],[16,30],[18,25],[20,4],[11,-11],[9,-19],[19,-49],[31,-60],[11,-41],[8,-49],[4,-54],[2,-56],[-8,-64],[-20,-35],[-19,-23],[-13,-40],[-19,-32],[-6,-7],[-5,-15],[-5,-104],[-29,-77],[-35,3],[-39,30],[-42,5],[-59,-36],[-39,-9],[-76,-53],[-35,20],[-29,-36],[-58,28],[-70,-35],[-23,-7],[-23,-31],[-16,-40],[-14,-46],[-16,-38],[-20,-15],[-22,-2],[-13,-6],[-7,-24],[-6,-60],[-15,-70],[-30,-33],[-33,-12],[-5,34],[-6,11],[-5,-4],[-3,-21],[0,-28],[-23,-22],[-3,0],[0,216],[-39,0],[1,14],[6,14],[13,27],[12,7],[11,62],[5,34],[-6,28],[-1,75],[-19,-11],[-9,-39],[-5,-31],[-7,-35],[-7,13],[-10,15],[-14,-16],[-6,-34],[-2,-29],[4,-30],[5,-5],[6,-4],[3,3],[3,10],[2,12],[2,-13],[4,-30],[-12,-19],[-1,-21],[-17,1],[-46,-97],[-4,-109],[-48,-34],[-90,-17]],[[7606,2602],[4,-23],[4,-14],[5,-10],[7,-5],[9,10],[9,15],[3,0],[3,-7],[9,-42],[6,-15],[7,-10],[14,-8],[7,-7],[9,-2],[9,6],[25,38],[10,-1],[5,-48],[1,-22],[1,-34],[-2,-43],[0,-12],[5,-11],[8,-13],[43,-30],[17,-22],[43,-75],[32,-29],[8,-11],[3,1],[2,4],[3,7],[2,4],[4,3],[3,-1],[3,-4],[3,-7],[3,-20],[1,-14],[-5,-62],[11,-3],[21,-24],[23,-12],[11,-11],[19,-26],[25,-17],[8,-12],[10,-20],[27,-83],[23,-106]],[[8107,1774],[-24,-97],[-13,-31],[-7,-1],[-3,12],[-5,38],[-5,28],[-1,16],[0,18],[1,17],[0,18],[-2,18],[-5,15],[-9,17],[-7,-2],[-4,-4],[-1,-13],[4,-54],[0,-26],[-2,-26],[-3,-19],[-4,-17],[-6,-17],[-3,-14],[-1,-11],[-1,-28],[-1,-13],[-3,-19],[-5,-6],[-9,0],[-20,11],[-9,3],[-15,-2],[-7,-18],[-5,-29],[-20,-234],[0,-13],[0,-9],[1,-29],[-14,-43],[-8,-26],[-6,-11],[-1,-15],[3,-20],[5,-86],[4,-245],[-10,-54],[-27,-74],[-13,-29],[-14,-21],[-11,-13],[-9,-7],[-6,-2],[-22,5],[-26,13],[-6,-11],[0,-8],[1,-17],[8,-58],[-2,-14],[-6,-5],[-26,18],[-7,9],[-6,4],[-2,-5],[-3,-25],[-3,-6],[-22,-1],[-16,-7],[-8,-1],[-5,2],[-2,3],[-3,4],[-16,11],[-7,9],[-6,12],[-8,28],[-6,13],[-5,0],[-18,-13],[-5,-6],[-3,-7],[-2,-9],[-3,-11],[-3,0],[-3,6],[-3,9],[-2,5],[-3,3],[-3,3],[-3,3],[-3,5],[-4,11],[-2,7],[-9,25],[-15,-27],[-15,-47],[-9,-12],[-15,-8],[-20,-1],[-13,3],[-22,-4],[-7,-4],[-4,-7],[-35,-140],[-5,-10],[-7,-6],[-54,-17],[-8,-8],[0,-9],[1,-9],[3,-13],[8,-51],[1,-66],[0,-1]],[[7317,174],[-50,-6],[-23,11],[-46,55],[-24,13],[-13,-29],[-6,0],[-7,20],[-9,4],[-10,-12],[-6,-30],[-23,23],[-27,6],[-24,-18],[-15,-52],[26,0],[0,-20],[-17,-2],[-26,-30],[-17,-8],[-86,-7],[-58,-42],[-34,-10],[-39,9],[-57,-46],[-18,-3],[-12,6],[-13,17],[-10,26],[-6,32],[1,54],[18,96],[1,27],[6,6],[12,4],[7,10],[6,18],[6,30],[7,12],[-3,31],[4,23],[18,47],[20,84],[2,15],[3,22],[44,119],[-2,25],[-12,37],[2,17],[8,-1],[24,-24],[9,26],[18,17],[4,11],[3,18],[16,72],[16,33],[40,63],[15,66],[18,38],[93,130],[18,8],[13,22],[15,105],[13,34],[3,1],[3,0],[6,-1],[-5,-34],[6,-17],[11,-1],[14,10],[13,24],[9,37],[7,43],[2,47],[7,17],[38,71],[3,139],[6,21],[7,19],[12,102],[-28,137],[-9,25],[-25,49],[-33,38],[-9,4],[6,-33],[-6,-20],[-17,35],[-14,20],[-10,29],[-4,65],[3,20],[8,11],[8,9],[7,10],[19,80],[0,19],[-34,63],[-17,12],[-7,-55],[-19,54],[-15,73],[-18,64],[-21,21]],[[7037,2584],[1,1],[3,8],[15,19],[4,7],[1,7],[1,7],[3,18],[3,17],[9,31],[2,14],[2,17],[1,31],[3,13],[3,3],[5,-16],[4,-28],[4,0],[6,9],[14,30],[7,10],[6,0],[4,-18],[2,-17],[21,-18],[91,-8],[34,-37],[13,-28],[6,-6],[8,0],[16,15],[12,1],[14,6],[20,21],[39,-21],[7,3],[19,1],[39,-14],[21,1],[14,-11],[8,-20],[18,-109],[9,-12],[16,38],[3,8],[38,45]],[[8748,2682],[4,-35],[-18,0],[-30,25],[-17,7],[-14,23],[-10,37],[0,52],[45,24],[11,-4],[9,-22],[13,-75],[7,-32]],[[8850,1731],[-24,96],[-14,38],[-6,23],[-1,10],[4,7],[3,8],[2,10],[2,10],[1,10],[2,10],[2,9],[3,6],[7,7],[1,4],[2,14],[3,72],[-1,19],[-2,9],[-8,-12],[-5,-5],[-4,-2],[-11,0],[-40,-23],[-18,16],[-14,35],[-6,4],[-4,-3],[-3,-8],[-3,-6],[-6,-5],[-3,-5],[-3,-7],[-4,-13],[-18,-56],[-14,-33],[-7,-13],[-4,-3],[-12,19],[-6,6],[-7,10],[-2,-3],[-2,-10],[-2,-12],[0,-9],[1,-9],[5,-32],[-3,-26],[-3,-21],[-40,-82],[-39,-11],[-29,13],[-11,-3],[-7,-6],[-67,-92],[-29,8],[-16,23],[-3,10],[-1,8],[-2,10],[-3,47],[-16,12],[-27,55],[-7,17],[-3,13],[1,6],[2,8],[3,12],[2,14],[1,20],[-2,7],[-7,4],[-11,-1],[-16,5],[-7,8],[-3,11],[4,31],[-9,42],[-43,96],[-13,17],[-2,-3],[-2,-19],[2,-25],[6,-46],[1,-20],[-2,-15],[-17,-20],[-4,-8],[-12,-31],[-14,-29],[-3,-10],[1,-7],[3,-4],[3,-9],[1,-28],[6,-43],[-2,-66],[-6,-62],[-2,-9],[-17,-49],[-3,22],[7,46],[1,13],[-1,5],[-4,-4],[-4,-1],[-5,4],[-9,13],[-7,8],[-7,4],[-9,1],[-22,-3]],[[7606,2602],[9,9],[9,19],[2,13],[1,18],[-3,28],[-5,21],[-10,33],[-2,16],[-1,23],[2,38],[2,26],[7,39],[17,65],[3,28],[2,40],[-1,206],[11,99],[11,-15],[10,-24],[5,-10],[4,-5],[5,-2],[7,2],[15,10],[49,62],[12,10],[8,1],[6,-6],[4,-2],[7,3],[3,8],[3,43],[11,82],[17,52],[23,54],[5,8],[15,37],[41,78],[20,25],[25,46],[5,19],[2,19],[-4,52],[1,18],[5,19],[3,10],[5,9],[7,48],[4,164],[0,4]],[[7983,4142],[22,-53],[13,-25],[98,-110],[30,-58],[4,-69],[3,-23],[-1,-63],[4,-35],[9,-30],[6,2],[10,28],[15,25],[10,7],[13,-10],[10,-17],[6,-20],[4,-28],[0,-36],[5,0],[2,14],[3,14],[2,13],[7,-44],[7,-10],[6,22],[-1,52],[8,-4],[25,-1],[11,5],[6,12],[15,38],[7,8],[12,-2],[12,-7],[8,-14],[8,-51],[8,-4],[5,12],[-5,26],[48,101],[5,-3],[14,-19],[3,-7],[3,-22],[9,-2],[17,14],[32,0],[26,-13],[18,-46],[8,-100],[37,22],[21,-40],[32,-143],[-19,-119],[-6,-20],[-10,-25],[-4,-16],[-1,-19],[-3,-44],[-2,-17],[-7,-33],[-13,-38],[-15,-27],[-16,1],[-4,28],[-4,44],[-5,28],[-12,-21],[-7,14],[-6,3],[-6,-6],[-5,-11],[7,-3],[6,-6],[3,-12],[2,-20],[-26,-6],[0,-35],[12,-50],[8,-50],[6,0],[10,46],[25,21],[27,4],[20,-8],[39,-58],[6,-14],[1,-41],[3,-30],[8,-18],[13,0],[-7,-69],[-13,0],[-15,29],[-13,19],[-7,7],[-3,17],[-2,21],[-4,18],[-7,16],[-2,5],[-51,-23],[-18,-19],[-61,21],[5,-33],[-1,-38],[-9,-70],[0,-18],[0,-36],[-3,-35],[-4,-30],[8,-26],[11,-63],[36,-59],[14,-15],[6,31],[11,63],[44,110],[9,58],[15,-35],[9,-45],[2,-50],[-7,-50],[24,8],[9,-9],[-4,-30],[-2,-28],[9,-20],[21,-21],[-5,65],[54,-38],[15,53],[5,0],[5,-16],[6,-16],[7,-15],[8,-13],[7,60],[-15,18],[-8,4],[-10,-2],[12,45],[22,19],[24,2],[18,-7],[16,-20],[19,-35],[7,-34],[-22,-12],[8,-37],[4,-40],[6,0],[2,12],[5,27],[8,-62],[22,-15],[52,18],[-16,31],[-35,52],[-18,14],[12,38],[22,17],[25,1],[17,-14],[-7,21],[15,-3],[43,-27],[11,-19],[3,-20],[-15,-12],[0,-20],[26,-20],[2,6],[0,4],[1,4],[3,6],[11,-23],[16,12],[15,-2],[8,-68],[2,15],[4,27],[1,19],[9,-19],[8,-12],[8,-1],[7,13],[-13,39],[74,28],[18,-18],[9,-15],[9,-6],[8,-11],[3,-27],[6,-17],[15,-1],[23,8],[15,-33],[13,-39],[17,-33],[73,-26],[19,-18],[15,-30],[-9,-16],[-4,-4],[13,-69],[0,-32],[3,-8],[4,-1],[6,-11],[6,-14]],[[7037,2584],[-6,7],[-26,-7],[-29,-21],[-24,-31],[-12,-41],[-9,17],[-5,-1],[-5,-8],[-7,-8],[-9,-7],[-6,-8],[-6,-5],[-11,1],[-16,8],[-47,51],[-36,-57],[-8,-2],[-13,30],[-7,9],[-7,4],[-27,-4],[-7,3],[-18,15],[-6,11],[-4,-6],[-12,-3],[-45,8],[-16,15],[-24,50],[-9,7]],[[7400,4741],[13,-29],[34,-47],[51,-32],[12,-28],[-6,-39],[19,0],[-6,-60],[-7,-30],[-11,-10],[-33,-6],[-13,-16],[-10,-28],[-3,-41],[13,-72],[47,73],[-3,-72],[9,19],[4,30],[3,35],[3,35],[-6,0],[13,57],[4,29],[1,35],[57,-58],[5,1],[8,13],[4,-5],[15,-30],[25,-18],[10,-2],[15,-19],[12,-41],[6,-41],[-7,-18],[-15,-10],[-52,-70],[4,44],[-9,25],[-9,2],[1,-31],[-4,-46],[12,-19],[19,-9],[15,-17],[14,-32],[15,-21],[18,-11],[19,-4],[0,18],[-20,21],[-6,13],[-5,26],[-7,41],[1,19],[18,21],[16,1],[22,-20],[20,-36],[12,-46],[6,0],[0,61],[6,0],[-3,-116],[-11,-38],[-49,-7],[0,-20],[19,0],[-9,-15],[-3,-4],[7,-38],[9,2],[10,14],[11,2],[8,-20],[10,-50],[8,-10],[5,6],[6,15],[6,16],[2,14],[2,9],[6,1],[6,-2],[5,1],[27,47],[10,12],[-14,15],[-30,12],[-12,13],[-11,39],[-3,47],[6,39],[17,16],[111,-10],[22,-21],[14,-32],[29,-34],[3,-7]],[[8844,159],[-1,0],[-10,3],[-31,21],[-9,14],[-6,3],[-3,-5],[-8,-27],[-5,-9],[-25,-10],[-30,2],[-28,16],[-19,33],[-31,-13],[-37,60],[-39,80],[-32,50],[-20,7],[-66,-7],[-18,7],[-36,28],[-19,7],[-35,-12],[-19,-16],[-12,-23],[-14,-6],[-122,88],[-83,28],[-42,-4],[-74,-60],[-35,-15],[-68,0],[-19,-8],[-39,-40],[-21,-13],[-117,21],[-120,-31],[-19,-29],[-20,3],[-21,13],[-19,5],[-14,-19],[-30,-72],[-20,-29],[-21,-16],[-70,-10]],[[2634,9772],[3,-28],[-1,-29],[5,-26],[15,-57],[-5,-12],[-8,-1],[-3,-10],[1,-17],[2,-26],[0,-12],[-5,-12],[-3,-9],[-1,-13],[-1,-49],[-9,-48],[-32,112],[-6,11],[-8,12],[-4,-4],[-3,-9],[-2,-10],[-1,-15],[-4,-36],[-6,-17],[-6,-10],[-3,1],[-6,27],[-6,17],[-5,-2],[-1,-4],[1,-8],[1,-11],[0,-14],[0,-15],[-3,-24],[0,-19],[2,-25],[7,-47],[2,-14],[-1,-11],[-32,-101],[-7,-38],[-5,-22],[-8,-9],[-11,-1],[-9,4],[-10,13],[-24,-5],[-36,10],[-9,-24],[-3,-21],[-12,-31],[-7,-8],[-5,3],[-10,36],[-6,12],[-7,-6],[-15,-18]],[[2324,9102],[-9,97],[-2,7],[-3,8],[-2,-3],[-2,-1],[-34,42],[-7,15],[-3,13],[1,6],[18,45],[8,34],[-1,27],[-10,37],[-7,6],[-6,-5],[-5,-13],[-11,-17],[-16,21],[-4,20]],[[2229,9441],[6,10],[20,18],[5,9],[9,41],[5,10],[12,3],[8,10],[15,26],[27,62],[15,25],[36,17],[43,53],[65,31],[29,36],[64,-45],[20,6],[26,19]],[[2099,8008],[-10,-23],[-8,36],[7,16],[4,17],[7,-31],[0,-15]],[[2247,7963],[-4,-18],[-4,12],[2,183],[-2,24],[-1,16],[4,2],[4,-12],[4,-22],[1,-12],[3,-35],[3,-67],[-5,-51],[-5,-20]],[[2324,9102],[-2,-100],[0,-78],[-11,-71],[-8,-95],[0,-107],[19,-113],[19,-77],[12,-20],[12,-22],[3,-37],[-3,-62]],[[2365,8320],[-61,-23],[-49,35],[-11,-7],[-18,-27],[-9,-6],[-43,27],[-32,-23],[-98,77],[-20,-2],[-19,-28],[-9,-48],[-4,-56],[0,-57],[-6,-53],[-13,-27],[-9,-8]],[[1854,9183],[1,4],[10,72],[13,62],[30,2],[33,-16],[23,11],[20,21],[84,-11],[86,33],[22,27],[19,-4],[34,57]],[[3143,6488],[2,-40],[-7,-2],[-10,17],[-9,-2],[-6,4],[-4,18],[-2,22],[0,24],[6,12],[10,-7],[9,-15],[5,-9],[6,-22]],[[3795,9745],[15,-38],[0,40],[7,0],[9,-43],[5,-19],[0,-14],[-8,-23],[-15,-27],[-11,3],[-10,15],[-12,9],[-30,-9],[-22,-23],[-46,-68],[11,34],[56,108],[16,10],[18,-2],[20,-13],[-4,16],[-5,31],[-4,15],[6,18],[4,-20]],[[4089,9787],[8,-3],[24,3],[5,3],[6,8],[6,-1],[2,-21],[-2,-21],[-5,-12],[-8,-11],[-7,-16],[-10,4],[-16,21],[-19,16],[-15,-10],[-4,17],[-4,1],[-11,-18],[3,40],[6,24],[10,12],[15,3],[4,-5],[7,-26],[5,-8]],[[3962,9812],[-2,-20],[-4,1],[-3,-2],[-1,-21],[-5,-4],[-11,7],[-7,-12],[-1,-19],[-4,9],[0,37],[9,32],[16,18],[8,6],[3,-9],[2,-23]],[[4027,9424],[-6,-6],[-14,-83],[-3,-67],[1,-30],[-1,-48],[-4,-23],[-7,-16],[-9,-8],[-6,-2],[-4,2],[-8,17],[-4,5],[-3,2],[-5,-1],[-5,-3],[-13,-15],[-4,-1],[-23,2],[-5,-8],[-3,-12],[-2,-28],[-1,-23],[1,-32],[3,-45],[1,-23],[-2,-42],[-5,-21],[-5,-14],[-6,-11],[-1,-11],[1,-8],[4,-6],[3,-7],[2,-9],[1,-19],[2,-14],[3,-14],[9,-30],[6,-25],[3,-15],[2,-12],[4,-10],[16,-30],[18,-47],[15,3],[7,9],[18,39],[18,20],[9,0],[7,-8],[3,-11],[4,-26],[1,-10],[4,-25],[6,-60],[14,-70],[3,-22],[-2,-14],[-4,-9],[-9,-11],[-5,-22],[-1,-16],[1,-13],[7,-21],[4,-20],[2,-19],[0,-25],[-5,-137],[1,-21],[2,-12],[5,-14],[4,-22],[2,-58],[3,-29],[4,-29],[6,-18],[13,-68]],[[4027,6476],[-41,-4],[-114,24],[-133,-44],[-81,24],[-18,18],[-34,52],[-37,22],[-14,29],[-9,45],[-24,147],[-3,28],[-1,46],[7,124],[-4,8],[-24,111],[-13,23],[-12,8],[-11,-10],[-6,-30],[0,-36],[13,-125],[1,-353],[-7,-90],[-19,4],[13,64],[-4,91],[-13,85],[-9,20],[-26,-27],[-6,-12],[-3,-25],[2,-54],[-4,-23],[-11,-14],[-8,11],[-6,17],[-7,7],[-7,-14],[-1,-20],[-3,-18],[-11,-8],[-34,44],[-20,194],[-35,43],[-45,0],[-10,9],[-18,38],[-10,9],[-7,-9],[-14,-38],[-10,-9],[-3,-3],[-2,-7],[-2,-8],[-6,-3],[-3,5],[-2,25],[-4,10],[-39,37],[-4,9],[-11,14],[-11,0],[-5,-32],[-9,-11],[-21,-3],[-21,6],[-13,17],[-6,0],[-2,-35],[-6,-10],[-17,8],[2,-12],[2,-8],[2,-20],[-10,0],[-43,16],[-13,14],[-23,76],[-9,41],[-3,42],[-16,49],[-131,154],[-14,23],[-14,33],[-8,14],[-18,6],[-8,8],[-4,17],[-11,53],[-1,12],[-13,-1],[-17,6],[-15,23],[-9,49],[5,46],[11,28],[14,17],[12,11],[106,-11],[35,26],[25,-11],[28,7],[17,9],[24,-18],[21,-5],[48,22],[37,45],[10,-5],[10,15],[18,0],[10,7],[13,11],[12,36],[9,24],[9,11],[-1,61],[3,27],[-2,46],[8,81],[-1,32],[-6,7]],[[3050,7991],[1,6],[1,4],[1,2],[3,4],[4,9],[3,9],[12,56],[8,30],[5,25],[3,22],[2,20],[7,147],[-2,43],[-15,40],[-35,62],[-5,28],[0,44],[10,73],[0,19],[-4,16],[-12,24],[-14,39],[-15,79],[12,109],[2,83],[1,12],[0,20],[-1,9],[-11,46],[-15,57],[-7,19],[-7,16],[-7,7],[-10,4],[-3,5],[-2,9],[3,23],[1,11],[1,5],[1,2],[5,14],[19,37],[8,35],[4,5],[2,8],[0,8],[-4,16],[-8,19],[-1,11],[2,14],[7,27],[4,8],[5,0],[9,-15],[3,-3],[1,1],[0,20],[-2,37],[-12,153],[10,35],[1,24]],[[3019,9683],[24,0],[14,6],[13,18],[10,-16],[10,-2],[11,1],[13,-5],[6,-11],[16,-38],[20,-33],[3,-58],[-3,-67],[-4,-51],[-6,-22],[-6,-12],[0,-8],[12,-17],[10,-4],[11,7],[13,17],[4,15],[6,70],[3,25],[8,20],[7,7],[8,1],[78,39],[10,13],[10,32],[23,19],[43,19],[14,14],[35,49],[22,50],[41,70],[15,13],[17,-19],[-13,-33],[-25,-31],[-42,-32],[-51,-89],[-13,-34],[0,-18],[41,-31],[53,-179],[33,-50],[19,-1],[7,13],[7,30],[14,48],[4,24],[10,72],[5,15],[10,3],[19,14],[6,2],[39,-19],[6,0],[4,5],[1,-10],[1,-46],[7,-64],[17,-16],[20,12],[19,17],[145,82],[23,1],[43,-26],[23,3],[20,24],[17,44],[5,55],[-17,56],[12,16],[13,5],[11,-6],[9,-15],[0,-18],[-8,-22],[-24,-139],[26,-56],[1,-7]],[[3917,9808],[1,-8],[-1,-18],[-5,-6],[-7,11],[-6,1],[-5,-18],[-3,-7],[-20,25],[-11,10],[-6,4],[-17,-6],[-10,2],[-27,28],[-9,7],[-7,27],[-5,8],[4,15],[18,-6],[26,-29],[32,-20],[58,-20]],[[3731,9999],[26,-45],[6,1],[4,-4],[3,-25],[-7,-24],[-10,5],[-11,10],[-11,4],[-10,13],[-5,20],[-4,13],[-8,8],[6,17],[21,7]],[[4973,8657],[48,-159],[20,-48],[43,-38],[34,-83],[38,-43],[19,-56],[15,-66],[8,-53],[-21,55],[-8,7],[-8,5],[-8,11],[-13,23],[-13,36],[-31,102],[-17,-18],[-17,5],[-16,23],[-21,48],[-6,6],[-4,13],[-1,33],[-3,11],[-15,33],[-4,6],[-8,19],[-5,41],[-8,41],[-17,18],[-28,-33],[-12,2],[-1,53],[26,24],[19,2],[15,-20]],[[4871,8818],[13,-22],[5,-20],[-8,-26],[-15,-7],[-6,19],[0,20],[-3,10],[-3,6],[-5,-3],[-7,5],[-18,37],[-8,28],[1,29],[12,10],[-2,18],[0,25],[5,10],[6,-33],[-2,-46],[6,-7],[18,-11],[6,-25],[5,-17]],[[4658,9129],[14,-21],[8,1],[-1,-39],[-10,-28],[-19,37],[-9,-8],[-10,-40],[-9,-13],[-7,72],[13,24],[15,15],[15,0]],[[4728,8970],[-11,-4],[-10,19],[-6,26],[2,18],[6,13],[2,15],[-8,4],[-8,13],[9,44],[21,16],[11,-10],[7,-30],[2,-21],[-6,-14],[-11,-10],[-7,-21],[5,-16],[7,-4],[2,-17],[-7,-21]],[[4343,9187],[8,-11],[6,-17],[3,-23],[1,-28],[-93,47],[-14,32],[6,13],[5,7],[6,-3],[8,-17],[45,65],[21,6],[16,-31],[-5,-8],[-9,-23],[-4,-9]],[[4027,9424],[34,-176],[19,-39],[14,-3],[38,-21],[14,-16],[9,-24],[9,-34],[13,-30],[16,-13],[52,0],[12,-10],[14,-36],[15,-24],[15,-17],[16,-10],[30,-3],[29,17],[55,63],[-7,70],[16,1],[49,-71],[22,-20],[18,10],[4,70],[10,-16],[10,-21],[8,-25],[3,-28],[21,-110],[2,-19],[35,-59],[5,-3],[16,3],[7,-7],[2,-14],[1,-14],[0,-7],[19,17],[30,83],[57,47],[8,-6],[0,-22],[-9,-23],[-17,-34],[-21,30],[-10,-24],[-13,-106],[10,-45],[6,-19],[9,-17],[10,-8],[38,-10],[16,-16],[55,-113],[3,-14],[3,-12],[6,-6],[5,7],[5,14],[4,15],[1,6],[21,8],[8,-15],[23,-295],[1,-1],[14,-14],[11,-16],[10,-24],[10,-33],[11,-46],[12,-90],[9,-43],[23,-77],[6,-13],[13,-8],[27,-1],[11,-9],[24,-111],[5,-34],[8,-18]],[[4516,9395],[3,-24],[3,-21],[4,-38],[-3,-8],[-5,22],[-4,14],[-4,-1],[-6,-4],[-7,3],[-6,13],[-3,16],[0,7],[3,-2],[2,16],[1,35],[10,7],[12,-35]],[[4444,9447],[-19,-25],[-18,11],[-40,54],[3,30],[4,21],[7,21],[5,29],[7,0],[6,-37],[10,-33],[13,-22],[12,-9],[15,-2],[5,-5],[-3,-13],[-7,-20]],[[4349,9475],[-5,-2],[-6,13],[-5,20],[-3,22],[-8,8],[-13,22],[-4,6],[-2,13],[-2,6],[-6,11],[-2,24],[9,17],[7,4],[7,2],[8,-7],[-1,-16],[7,-21],[14,-35],[5,-23],[3,-9],[3,-19],[-1,-20],[-5,-16]],[[4226,9656],[-7,-5],[-8,1],[-24,-8],[-8,-21],[-14,-18],[-13,-10],[-2,6],[-2,19],[4,6],[8,-3],[-2,26],[5,16],[13,5],[7,1],[21,5],[11,0],[13,-7],[-2,-13]],[[3050,7991],[-7,6],[-14,1],[-25,64],[-17,41],[-32,71],[-33,35],[-82,86],[-32,-3],[-74,-20],[-99,-49],[-29,38],[-63,52],[-70,-1],[-40,-13],[-20,-7],[-19,6],[-29,22]],[[2634,9772],[11,8],[22,7],[9,6],[10,12],[10,5],[9,-13],[35,-72],[27,-22],[59,-17],[28,-20],[20,19],[145,-2]]],"transform":{"scale":[0.0010817813552355272,0.00034380879497950754],"translate":[-84.9496150379999,19.827826239000046]}};
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
  Datamap.prototype.ginTopo = '__GIN__';
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
