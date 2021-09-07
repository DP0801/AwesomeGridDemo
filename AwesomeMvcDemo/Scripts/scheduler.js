var scheduler = function ($) {
    function buildCell(i) {
        return function (row) {
            var cell = row.Cells[i];
            if (!cell) return "";
            var result = "<div data-ticks='" + cell.Ticks + "' data-allday='" + row.AllDay + "' class='timePart' >";

            if (cell.Day) {
                result += "<div class='awe-il'><span class='day' data-date='" + cell.Date + "'>" + cell.Day + "</span></div>";
            }

            if (cell.Events) {
                awef.loop(cell.Events, function (value) {
                    var style = '';
                    if (value.Color) {
                        var color = 'fff';
                        var bcolor = value.Color.replace('#', '');

                        if (hexToVal(bcolor) > 530) {
                            color = darken(bcolor);
                        }

                        style = 'style="background-color:' + value.Color + '; color:#' + color + ';"';
                    }
                    result += '<div class="schEvent" data-id="' + value.Id + '" ' + style + '>'
                        + (value.Time ? '<div class="schTime">' + value.Time + '</div>' : "")
                        + '<button type="button" class="delEvent">&times;</button>'
                        + '<div class="eventTitle">' + value.Title + '</div>'
                        + '</div>';
                });
            }

            result += '</div>';
            return result;
        };
    }

    function hexToVal(hex) {
        var r = parseInt(hex.substr(0, 2), 16),
            g = parseInt(hex.substr(2, 2), 16),
            b = parseInt(hex.substr(4, 2), 16);
        return r + g + b;
    }

    function darken(hex) {
        var r = parseInt(hex.substr(0, 2), 16),
            g = parseInt(hex.substr(2, 2), 16),
            b = parseInt(hex.substr(4, 2), 16);

        function f(c) {
            var h = Math.max(0, c - 170).toString(16);
            if (!h) h = '00';
            if (h.length == 1) h = '0' + h;
            return h;
        }

        return f(r) + f(g) + f(b);
    }

    return {
        buildCell: buildCell,
        init: function (gridId, popupName) {
            var g = $('#' + gridId);
            var o = g.data('o');
            var sched = g.closest('.scheduler');
            var api = o.api;
            var $viewType = sched.find('.viewType .awe-val');
            var fzOpt = { left: 1 };
            var bar = $('.schedBotBar[data-g='+gridId+']');

            awem.gridFreezeColumns(fzOpt)(o);

            sched.find('.prevbtn').click(function () {
                api.load({ oparams: { cmd: 'prev' } });
            });

            sched.find('.nextbtn').click(function () {
                api.load({ oparams: { cmd: 'next' } });
            });

            sched.find('.todaybtn').click(function () {
                api.load({ oparams: { cmd: 'today' } });
            });

            g.on('awebfren', function () {
                var tag = o.lrs.tg;

                if (tag.View === 'Agenda' || tag.View === 'Month') {
                    fzOpt.left = 0;
                    bar.hide();

                } else {
                    fzOpt.left = 1;
                    bar.show();
                }
            });

            g.on('aweload', function (e) {
                var data = o.lrs;
                var tag = data.tg;

                if ($viewType.val() !== tag.View) {
                    $viewType.val(tag.View).data('api').render();
                }

                sched.find('.schDate .awe-val').val(tag.Date);
                sched.find('.dateLabel').html(tag.DateLabel);
            })
                .on('click', '.eventTitle', function () {
                    awe.open('edit' + popupName, { params: { id: $(this).parent().data('id') } });
                })
                .on('click', '.delEvent', function () {
                    awe.open('delete' + popupName, { params: { id: $(this).closest('.schEvent').data('id') } });
                })
                .on('dblclick', 'td', function (e) {
                    var schdev = $(e.target).closest('.schEvent');
                    if (!schdev.length) {
                        var tp = $(this).find('.timePart');
                        awe.open('create' + popupName,
                            { params: { ticks: tp.data('ticks'), allDay: tp.data('allday') } });
                    } else {
                        if (!$(e.target).is('.delEvent'))
                            awe.open('edit' + popupName, { params: { id: schdev.data('id') } });
                    }
                })
                .on('click', '.day', function (e) {
                    if ($(e.target).is('.day')) {
                        api.load({ oparams: { viewType: 'Day', date: $(this).data('date') } });
                    }
                });
        }
    };
}(jQuery);