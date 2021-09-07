var signalrSync = function () {
    function regGridSync() {
        function init() {
            if ($('.awe-grid[data-syncg]').length) {
                initSync();
            }
        }

        $(document).ajaxComplete(init);
        init();
    }

    function initSync() {
        if (document.isync) return;
        document.isync = 1;
        var root = document.root;

        $.ajax(root + '/Scripts/jquery.signalR-2.3.0.min.js',
            {
                dataType: "script",
                cache: true,
                success: function () {
                    $.ajax(root + '/signalr/hubs',
                    {
                        dataType: "script",
                        cache: true,
                        success: function () {
                            try {
                                init();
                            } catch (err) {
                            }
                        }
                    });
                }
            });

        function init() {
            var hub = $.connection.syncHub;
            var connId;

            hub.client.broadcastMessage = function (srccid, srcgid, key, act, group) {
                $('.awe-grid[data-syncg="' + group + '"]').each(function (i, el) {
                    updateRow($(el), srccid, srcgid, key, act);
                });
            };

            $.connection.hub.start().done(function () {
                connId = $.connection.hub.id;
                $(document)
                    .on('itemdelete', function (e) {
                        send(e, 'del');
                    })
                    .on('aweinlinesave itemedit', '.awe-row', function (e) {
                        send(e, '');
                    });
            });

            function send(e, act) {
                var key = $(e.target).data('k').toString();
                var grid = $(e.target).closest('.awe-grid');
                var srcgid = grid.attr('id');
                trysend(function () { hub.server.send(connId, srcgid, key, act, grid.data('syncg')); });
            }

            function trysend(action, attempts) {
                attempts = attempts || 0;
                try {
                    action();
                } catch (err) {
                    if (attempts < 1) {
                        $.connection.hub.start();
                        setTimeout(function () {
                            trysend(action, attempts + 1);
                        }, 300);
                    }
                }
            }

            function updateRow(g, srccid, srcgid, key, act) {
                var gid = g.attr('id');
                if (srccid == connId && srcgid == gid) return;
                var row = g.find('.awe-row[data-k="' + key + '"]');
                if (row.length && !row.hasClass('o-glrow')) {
                    if (act == 'del')
                        utils.delRow(row);
                    else
                        utils.itemEdited(gid, 1, 1)({ id: key });
                }
            }
        }
    }

    return {
        init: regGridSync
    };
}();