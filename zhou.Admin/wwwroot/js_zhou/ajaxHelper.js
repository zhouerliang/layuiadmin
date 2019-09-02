var ajaxHelper = {
    /* 封装ajax函数
    * @param {string}opt.type http连接的方式，包括POST和GET两种方式
    * @param {string}opt.url 发送请求的url
    * @param {boolean}opt.async 是否为异步请求，true为异步的，false为同步的
    * @param {object}opt.data 发送的参数，格式为对象类型
    * @param {function}opt.success ajax发送并接收成功调用的回调函数
    */
    ajax: function (opt) {
        console.log(opt.method);
        opt = opt || {};
        opt.method = opt.method.toUpperCase() || 'POST';
        opt.url = opt.url || '';
        opt.async = opt.async || true;
        opt.data = opt.data || null;
        opt.success = opt.success || function () { };

        $.ajax({
            url: opt.url,
            data: opt.data,
            type: opt.method,
            async: opt.asnyc,
            dataType: "json",
            success: opt.success,
            error: function (obj) {
                console.group("ajaxHelper.ajax.error");
                console.error(obj);
                console.groupEnd();
            }
        });

    },
    post: function (url, data, success) {
        ajaxHelper.ajax({
            url: url,
            data: data,
            method: "POST",
            success: success
        });
    },
    get: function (url, data, success) {
        ajaxHelper.ajax({
            url: url,
            data: data,
            method: "GET",
            success: success,
        });
    }
};