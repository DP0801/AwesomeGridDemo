
//import { awef } from '../aweui/all.js';

var aweq = function () {
    var loop = awef.loop, isNotNull = awef.isNotNull, isNull = awef.isNull, isNullOrEmp = awef.isNullOrEmp, where = awef.where;
    function toKeyContArr(arr, keyp, conp) {
        var res = [];

        loop(arr, function (o) {
            res.push(newKeyCon(o[keyp], o[conp]));
        });

        return res;
    }

    function equalsn(v1, v2) {
        return isNullOrEmp(v2) || !isNullOrEmp(v1) && v1.toString() === v2.toString();
    }

    function arrContains(list, val) {
        var res = false;
        val = val.toString();
        loop(list, function(it) {
            if (it.toString() === val) {
                res = true;
                return false;
            }
        });

        return res;
    }

    function first(list, func) {
        return where(list, func)[0];
    }
    
    function getById(list, id) {
        id = id.toString();
        return where(list, function (o) { return o.id.toString() === id; })[0];
    }

    // id: string
    function removeById(list, id) {
        var index;

        loop(list, function (o, ix) {
            if (o.id.toString() === id) {
                index = ix;
            }
        });

        if (isNotNull(index)) {
            list.splice(index, 1);
        }
    }
    
    function getByIds(list, ids) {
        if (isNotNull(ids) && !(ids instanceof Array)) {
            ids = [ids];
        }

        var res = [];
        loop(ids, function (id) {
            var fit = first(list,
                function (it) {
                    return it.id.toString() === id.toString();
                });

            if (isNotNull(fit)) {
                res.push(fit);
            }
        });

        return res;
    }

    function takePage(list, page, pageSize) {
        var skipsize = (page - 1) * pageSize;
        return list.slice(skipsize, skipsize + pageSize);
    }

    function newKeyCon(key, con) {
        return { k: key, c: con };
    }

    return {
        toKeyContArr: toKeyContArr,
        newKeyCon: newKeyCon,
        takePage: takePage,
        first: first,
        where: where,
        contains: awef.scont,
        select: awef.select,
        getById: getById,
        getByIds: getByIds,
        removeById: removeById,
        arrContains: arrContains,
        equalsn: equalsn
    };
}();

//export { aweq };