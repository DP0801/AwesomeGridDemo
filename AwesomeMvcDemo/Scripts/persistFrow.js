// persist filter row values after page refresh

function persistFilter(o) {
    var g = o.v;
    var storage = sessionStorage;
    var fkey = 'pers' + o.id;
    g.on('awefinit', function () {
        var fopt = storage[fkey];
        if (fopt) {
            fopt = JSON.parse(fopt, function(key, val) {
                if(val && val.length > 0 && val[0] === '[')
                {
                    return JSON.parse(val);
                }

                return val;
            });

            if (fopt.model) {
                o.fltopt.model = fopt.model;
                o.fltopt.order = fopt.order;
            }
        }

        g.on('aweload', function (e) {
            if ($(e.target).is(g)) {
                fopt = o.fltopt;
                storage[fkey] = JSON.stringify({ model: fopt.model, order: fopt.order });
            }
        });
    });
}