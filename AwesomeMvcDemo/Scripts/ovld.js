var ovld = function ($) {
    return {
        ignore: function (inp) {
        },
        rules: {
            req: function (o) { return !o.v; },
            len: function (max, min) {
                return function (o) {
                    var v = o.v, e = o.e;
                    return v && ((max !== null && v.length > max) ||
                        (min !== null && v.length < min && (!e || e.type !== 'keyup')));
                }
            }
        },
        bind: function (opt) {
            var tched = { };

            function chkInp(inp, e) {
                if (opt.ignore && opt.ignore(inp) || ovld.ignore(inp)) return;
                var name = inp.attr('name');
                if (!name) return;

                tched[name] = inp;

                var res = chkRulesFor(name, e, inp);
                
                var related = opt.related;

                if (related) {
                    var nrel = related[name];
                    if (nrel) {
                        awef.loop(nrel, function(rname) {
                            var rinp = tched[rname];
                            if (rinp) {
                                res += chkRulesFor(rname, e, rinp);
                            }
                        });
                    }
                }

                return res;
            }

            function chkRulesFor(name, e, inp) {
                var res = 0;
                var rls = opt.rules[name];
                if (!rls) return res;
                if (!Array.isArray(rls)) rls = [rls];

                var val = inp.val().trim();
                var o = { v: val, e: e, inp: inp, name: name };
                var msgc = opt.msgCont(o);
                if (!msgc.length) return res;
                msgc.empty();

                awef.loop(rls, function (rule) {
                    if (rule.chk(o)) {
                        msgc.append('<div class="field-validation-error">' + rule.msg + '</div>');
                        res++;
                        return false;
                    }
                });

                return res;
            }

            var cont = $(opt.sel);
            cont.on('change keyup', 'input', function (e) {
                chkInp($(this), e);
            });

            if (opt.subev) {
                cont.on(opt.subev, function (e, evd) {
                    $(e.target).find('input:not(:checkbox)').each(function () {
                        if (chkInp($(this), e) && evd) {
                            evd.cancel = 1;
                            evd.target = this;
                        }
                    });
                });
            }
        }
    };
}(jQuery);

//export {ovld};