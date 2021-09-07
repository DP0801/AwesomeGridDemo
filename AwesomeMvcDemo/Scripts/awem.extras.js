awemex = function ($) {
    var $win = $(window);
    var se = ' ';
    var snbsp = '&nbsp;';
    var cd = awem.clienDict;

    function rbtn(cls, cont, attr) {
        if (attr == null) attr = se;
        return '<button type="button" class="' + cls + '" ' + attr + '>' + cont + '</button>';
    }

    function kp(item) {
        return item.k;
    }

    function rdiv(cls, cont, attr) {
        if (cont == null) cont = se;
        if (attr == null) attr = se;
        return '<div class="' + cls + '" ' + attr + '>' + cont + '</div>';
    }

    function uiPopup(o) {
        var soption = "option";
        var pp = o;
        var popup = pp.d;
        var cx = {};
        o.cx = cx;

        pp.mlh = 0;

        var autoSize = awe.autoSize;
        var fullscreen = pp.fullscreen;
        var draggable = true;

        if (!pp.r) pp.r = false;

        if (fullscreen) {
            pp.r = false;
            draggable = false;
            pp.modal = true;
        }

        pp.uiw = pp.width;
        if (!pp.uiw) pp.uiw = 700;

        popup.dialog({
            draggable: draggable,
            width: pp.uiw,
            height: pp.height,
            modal: pp.modal,
            resizable: pp.r,
            buttons: pp.btns,
            autoOpen: false,
            title: pp.title,
            resizeStop: function () {
                pp.uiw = popup.dialog(soption, 'width');
                pp.height = popup.dialog(soption, 'height');
                pp.p = popup.dialog(soption, 'position');
            },
            dragStop: function () {
                pp.p = popup.dialog(soption, 'position');
            }
        });

        var dialogClass = "awe-uidialog awe-popupw";
        if (o.rtl) {
            dialogClass += ' awe-rtl';
        }

        if (pp.pc) dialogClass = dialogClass + " " + pp.pc;
        popup.dialog(soption, { dialogClass: dialogClass });

        var autoResize = function () { };
        if (fullscreen || autoSize) {
            //autosize
            autoResize = function (e) {
                if (popup.is(':visible'))
                    if (!e || e.target == window || e.target == document) {

                        var wh = $win.height();
                        var ww = $win.width();

                        var sw = pp.uiw > ww - 10 || fullscreen ? ww - 10 : pp.uiw;
                        var sh = pp.height > wh - 5 || fullscreen ? wh - 20 : pp.height;
                        var opt = {
                            height: sh,
                            width: sw
                        };

                        //on ie9 it goes off screen on zoom when setting position
                        if (!fullscreen && pp.p) opt.position = pp.p;
                        popup.dialog(soption, opt).trigger('aweresize');
                    }
            };

            $win.on('resize', autoResize);
            autoResize();
            popup.on('change', autoResize);
        }//end if fullscreen or autoSize 

        popup.on('dialogclose', function () {
            popup.trigger('aweclose');

            cx.isOpen = 0;
            if (pp.cl) {
                pp.cl.call(o);
            }

            if (!pp.loadOnce) {
                if (autoSize || fullscreen) {
                    $win.off('resize', autoResize);
                }

                popup.find('*').remove();
                popup.remove();
            }

        }).on('dialogresize', function () {
            popup.trigger('aweresize');
        });

        popup.on('aweload awebeginload', function () {
            o.avh = 0;
            popup.trigger('aweresize');
        });

        var api = function () { };
        api.open = function () {
            popup.dialog('open');
            cx.isOpen = 1;
            popup.trigger('aweopen');
            autoResize();
        };
        api.close = function () {
            popup.dialog('close');
        };
        api.destroy = function () {
            api.close();
            $win.off('resize', autoResize);
            popup.remove();
        };
        
        cx.api = api;

        popup.data('api', api);
    }

    function bootstrapPopup(o) {
        var p = o; //popup properties
        var popup = p.d; //popup div
        var modal = $('<div class="modal" tabindex="-1" role="dialog" aria-hidden="true">' +
            '<div class="modal-dialog"><div class="modal-content awe-popupw"><div class="modal-header">' +
            rbtn('close', '&times;', 'data-dismiss="modal" aria-hidden="true"') +
            '<h4 class="modal-title"></h4></div></div></div></div>');

        var hasFooter = p.btns && p.btns.length;
        var cx = {};
        o.cx = cx;

        //minimum height of the lookup/multilookup content
        p.mlh = !p.fullscreen ? 250 : 0;
        
        if (!p.title) {
            p.title = snbsp; //put one space when no title
        }

        popup.addClass("modal-body");
        popup.css('overflow', 'auto');

        modal.find('.modal-content').append(popup);
        modal.find('.modal-title').html(p.title);
        popup.show();

        //use to resize the popup when fullscreen
        function autoResize() {
            var h = $win.height() - 120;
            if (hasFooter) h -= 90;
            if (h < 400) h = 400;
            popup.height(h);
            popup.trigger('aweresize');
        }

        var api = function () { };
        api.open = function () {
            modal.appendTo($('body')); //appendTo each time to prevent modal to show under current top modal
            modal.modal('show');
            cx.isOpen = 1;
            popup.trigger('aweopen');
            if (p.fullscreen) autoResize();
        };
        api.close = function () {
            modal.modal('hide');

            cx.isOpen = 0;
            if (p.cl) {
                p.cl();
            }
            if (!p.loadOnce) {
                popup.find('*').remove();
                popup.closest('.modal').remove();
            }
        };

        api.destroy = function () {
            api.close();
            $win.off('resize', autoResize);
            popup.closest('.modal').remove();
        };
        api.lay = function() {}

        cx.api = api;

        popup.data('api', api);

        modal.on('hidden.bs.modal', function () {
            popup.trigger('aweclose');
        });

        $('body').append(modal);

        //fullscreen
        if (p.fullscreen) {
            modal.find('.modal-dialog').css('width', 'auto').css('margin', '10px');
            $win.on('resize', autoResize);
        }

        //add buttons if any
        if (hasFooter) {
            var footer = $('<div class="modal-footer"></div>');
            modal.find('.modal-footer').remove();
            modal.find('.modal-content').append(footer);
            $.each(p.btns, function (i, e) {


                var btn = $(rbtn("btn btn-default", e.text));
                btn.click(function () { e.click.call(popup); });
                footer.append(btn);
            });
        }
    }

    function bootstrapDropdown(o) {
        function render() {
            o.d.empty();
            var caption = cd().Select;
            var items = se;
            $.each(o.lrs, function (i, item) {
                var checked = $.inArray(kp(item), awe.val(o.v)) > -1;
                if (checked) caption = cp(item);
                items += '<li role="presentation"><input style="display:none;" type="radio" value="' +
                    kp(item) + '" name="' + o.nm + '" ' + (checked ? 'checked="checked"' : se) +
                    '" /><a role="menuitem" tabindex="-1" href="#" >' + cp(item) + '</a></li>';
            });

            if (!items) items = '<li class="o-empt">(' + cd().Empty + ')</li>';

            var res = rdiv('dropdown', rbtn('btn btn-default dropdown-toggle',
                '<span class="caption">' + caption + '</span> <i class="caret"></i>',
                'data-toggle="dropdown" aria-expanded="true"')
                + '<ul class="dropdown-menu" role="menu">' + items + '</ul>');

            o.d.append(res);
        };

        o.v.data('api').render = render;

        o.v.on('change', render);

        o.d.on('click', 'a', function (e) {
            e.preventDefault();
            $(this).prev().click(); // click on the hidden radiobutton
        });
    }

    return {
        uiPopup: uiPopup,
        bootstrapPopup: bootstrapPopup,
        bootstrapDropdown: bootstrapDropdown
    };
}(jQuery);