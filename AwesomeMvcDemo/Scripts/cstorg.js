/* eslint-disable */
var cstorg = function () {
    var prefix = "awed.";
    var version = 1;
    var data = {};
    
    var pstorg = sessionStorage || {};

    if (document.ver) {
        init(document.ver, document.dev);
    }
    
    function init(v, dev) {
        version = v || version;
        
        if (dev) {
            pstorg = {};
        }

        // remove storage keys from older versions; current value is "awed.oldversion"
        try {
            for (var key in pstorg) {
                if (key.indexOf(prefix) === 0) {
                    if (key.indexOf(prefix + version) !== 0) {
                        pstorg.removeItem(key);
                    }
                }
            }
        } catch (err) {
            /*empty*/
        }
    }

    return {
        init: init,
        get: function(url, sd) {
            return this.getf(url, sd)();
        },
        getf: function (url, sd) {
            return function () {
                var prm = sd ? JSON.stringify(sd) : '';
                var pkey = prefix + version + url + prm;

                if (data[pkey]) {
                    return data[pkey];
                } else {
                    if (pstorg[pkey]) {
                        var res = data[pkey] = JSON.parse(pstorg[pkey]);
                        return res;
                    }

                    var xhr = $.get(url, sd);
                    data[pkey] = xhr;
                    return xhr.then(function(rs) {
                        data[pkey] = rs;
                        pstorg[pkey] = JSON.stringify(rs);
                        return rs;
                    });
                }
            }
        }
    };
}();

//export {cstorg};