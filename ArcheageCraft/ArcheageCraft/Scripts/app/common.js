
window.common = (function () {
    var common = {};

    common.getFragment = function getFragment() {
        if (window.location.hash.indexOf("#") === 0) {
            return parseQueryString(window.location.hash.substr(1));
        } else {
            return {};
        }
    };

    function parseQueryString(queryString) {
        var data = {},
            pairs, pair, separatorIndex, escapedKey, escapedValue, key, value;

        if (queryString === null) {
            return data;
        }

        pairs = queryString.split("&");

        for (var i = 0; i < pairs.length; i++) {
            pair = pairs[i];
            separatorIndex = pair.indexOf("=");

            if (separatorIndex === -1) {
                escapedKey = pair;
                escapedValue = null;
            } else {
                escapedKey = pair.substr(0, separatorIndex);
                escapedValue = pair.substr(separatorIndex + 1);
            }

            key = decodeURIComponent(escapedKey);
            value = decodeURIComponent(escapedValue);

            data[key] = value;
        }

        return data;
    }

    return common;
})();

ko.bindingHandlers.chart = {
    chart: null,
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        this.chart = new Highcharts.Chart({
            chart: {
                renderTo: element,
                zoomType: 'x'
            },
            xAxis: {
                type: 'datetime',
                dateTimeLabelFormats: { // don't display the dummy year
                    month: '%e. %b',
                    year: '%b'
                },

                title: {
                    text: 'Date'
                }
            },
            yAxis: {
                title: {
                    text: "Price"
                }
            },
            series: [{ data: [] }]
        });
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        var value = ko.unwrap(valueAccessor());
        var data = [];
        for (var i = 0; i < value.length; i++) {
            data.push({x:new Date(value[i].date),y:value[i].value})
        }
        chart.series[0].setData(data)
    }
};

ko.bindingHandlers.autoComplete = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var onRequest = allBindingsAccessor('onRequest'); // url to post to is read here
        var labelKeyName = allBindingsAccessor('textPath');
        var valueKeyName = allBindingsAccessor('valuePath');

        var selectedObservableArrayInViewModel = valueAccessor();

        $(element).autocomplete({
            minLength: 2,
            autoFocus: true,
            source: function (request, response) {
                onRequest.onRequest(request, response);
            },
            select: function (event, ui) {
               valueAccessor()
            }
        });
    }
};

