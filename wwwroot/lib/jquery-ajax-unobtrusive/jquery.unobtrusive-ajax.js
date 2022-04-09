// Unobtrusive Ajax support library for jQuery
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// @version v3.2.6
// 
// Microsoft grants you the right to use these script files for the sole
// purpose of either: (i) interacting through your browser with the Microsoft
// website or online service, subject to the applicable licensing or use
// terms; or (ii) using the files as included with a Microsoft product subject
// to that product's license terms. Microsoft reserves all other rights to the
// files not expressly granted by Microsoft, whether by implication, estoppel
// or otherwise. Insofar as a script file is dual licensed under GPL,
// Microsoft neither took the code under GPL nor distributes it thereunder but
// under the terms set out in this paragraph. All notices and licenses
// below are for informational purposes only.

/*jslint white: true, browser: true, onevar: true, undef: true, nomen: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, newcap: true, immed: true, strict: false */
/*global window: false, jQuery: false */

(function ($) ***REMOVED***
    var data_click = "unobtrusiveAjaxClick",
        data_target = "unobtrusiveAjaxClickTarget",
        data_validation = "unobtrusiveValidation";

    function getFunction(code, argNames) ***REMOVED***
        var fn = window, parts = (code || "").split(".");
        while (fn && parts.length) ***REMOVED***
            fn = fn[parts.shift()];
    ***REMOVED***
        if (typeof (fn) === "function") ***REMOVED***
            return fn;
    ***REMOVED***
        argNames.push(code);
        return Function.constructor.apply(null, argNames);
***REMOVED***

    function isMethodProxySafe(method) ***REMOVED***
        return method === "GET" || method === "POST";
***REMOVED***

    function asyncOnBeforeSend(xhr, method) ***REMOVED***
        if (!isMethodProxySafe(method)) ***REMOVED***
            xhr.setRequestHeader("X-HTTP-Method-Override", method);
    ***REMOVED***
***REMOVED***

    function asyncOnSuccess(element, data, contentType) ***REMOVED***
        var mode;

        if (contentType.indexOf("application/x-javascript") !== -1) ***REMOVED***  // jQuery already executes JavaScript for us
            return;
    ***REMOVED***

        mode = (element.getAttribute("data-ajax-mode") || "").toUpperCase();
        $(element.getAttribute("data-ajax-update")).each(function (i, update) ***REMOVED***
            var top;

            switch (mode) ***REMOVED***
                case "BEFORE":
                    $(update).prepend(data);
                    break;
                case "AFTER":
                    $(update).append(data);
                    break;
                case "REPLACE-WITH":
                    $(update).replaceWith(data);
                    break;
                default:
                    $(update).html(data);
                    break;
        ***REMOVED***
    ***REMOVED***);
***REMOVED***

    function asyncRequest(element, options) ***REMOVED***
        var confirm, loading, method, duration;

        confirm = element.getAttribute("data-ajax-confirm");
        if (confirm && !window.confirm(confirm)) ***REMOVED***
            return;
    ***REMOVED***

        loading = $(element.getAttribute("data-ajax-loading"));
        duration = parseInt(element.getAttribute("data-ajax-loading-duration"), 10) || 0;

        $.extend(options, ***REMOVED***
            type: element.getAttribute("data-ajax-method") || undefined,
            url: element.getAttribute("data-ajax-url") || undefined,
            cache: (element.getAttribute("data-ajax-cache") || "").toLowerCase() === "true",
            beforeSend: function (xhr) ***REMOVED***
                var result;
                asyncOnBeforeSend(xhr, method);
                result = getFunction(element.getAttribute("data-ajax-begin"), ["xhr"]).apply(element, arguments);
                if (result !== false) ***REMOVED***
                    loading.show(duration);
            ***REMOVED***
                return result;
          ***REMOVED***
            complete: function () ***REMOVED***
                loading.hide(duration);
                getFunction(element.getAttribute("data-ajax-complete"), ["xhr", "status"]).apply(element, arguments);
          ***REMOVED***
            success: function (data, status, xhr) ***REMOVED***
                asyncOnSuccess(element, data, xhr.getResponseHeader("Content-Type") || "text/html");
                getFunction(element.getAttribute("data-ajax-success"), ["data", "status", "xhr"]).apply(element, arguments);
          ***REMOVED***
            error: function () ***REMOVED***
                getFunction(element.getAttribute("data-ajax-failure"), ["xhr", "status", "error"]).apply(element, arguments);
        ***REMOVED***
    ***REMOVED***);

        options.data.push(***REMOVED*** name: "X-Requested-With", value: "XMLHttpRequest" ***REMOVED***);

        method = options.type.toUpperCase();
        if (!isMethodProxySafe(method)) ***REMOVED***
            options.type = "POST";
            options.data.push(***REMOVED*** name: "X-HTTP-Method-Override", value: method ***REMOVED***);
    ***REMOVED***

        // change here:
        // Check for a Form POST with enctype=multipart/form-data
        // add the input file that were not previously included in the serializeArray()
        // set processData and contentType to false
        var $element = $(element);
        if ($element.is("form") && $element.attr("enctype") == "multipart/form-data") ***REMOVED***
            var formdata = new FormData();
            $.each(options.data, function (i, v) ***REMOVED***
                formdata.append(v.name, v.value);
        ***REMOVED***);
            $("input[type=file]", $element).each(function () ***REMOVED***
                var file = this;
                $.each(file.files, function (n, v) ***REMOVED***
                    formdata.append(file.name, v);
            ***REMOVED***);
        ***REMOVED***);
            $.extend(options, ***REMOVED***
                processData: false,
                contentType: false,
                data: formdata
        ***REMOVED***);
    ***REMOVED***
        // end change

        $.ajax(options);
***REMOVED***

    function validate(form) ***REMOVED***
        var validationInfo = $(form).data(data_validation);
        return !validationInfo || !validationInfo.validate || validationInfo.validate();
***REMOVED***

    $(document).on("click", "a[data-ajax=true]", function (evt) ***REMOVED***
        evt.preventDefault();
        asyncRequest(this, ***REMOVED***
            url: this.href,
            type: "GET",
            data: []
    ***REMOVED***);
***REMOVED***);

    $(document).on("click", "form[data-ajax=true] input[type=image]", function (evt) ***REMOVED***
        var name = evt.target.name,
            target = $(evt.target),
            form = $(target.parents("form")[0]),
            offset = target.offset();

        form.data(data_click, [
            ***REMOVED*** name: name + ".x", value: Math.round(evt.pageX - offset.left) ***REMOVED***,
            ***REMOVED*** name: name + ".y", value: Math.round(evt.pageY - offset.top) ***REMOVED***
        ]);

        setTimeout(function () ***REMOVED***
            form.removeData(data_click);
      ***REMOVED*** 0);
***REMOVED***);

    $(document).on("click", "form[data-ajax=true] :submit", function (evt) ***REMOVED***
        var name = evt.currentTarget.name,
            target = $(evt.target),
            form = $(target.parents("form")[0]);

        form.data(data_click, name ? [***REMOVED*** name: name, value: evt.currentTarget.value ***REMOVED***] : []);
        form.data(data_target, target);

        setTimeout(function () ***REMOVED***
            form.removeData(data_click);
            form.removeData(data_target);
      ***REMOVED*** 0);
***REMOVED***);

    $(document).on("submit", "form[data-ajax=true]", function (evt) ***REMOVED***
        var clickInfo = $(this).data(data_click) || [],
            clickTarget = $(this).data(data_target),
            isCancel = clickTarget && (clickTarget.hasClass("cancel") || clickTarget.attr('formnovalidate') !== undefined);
        evt.preventDefault();
        if (!isCancel && !validate(this)) ***REMOVED***
            return;
    ***REMOVED***
        asyncRequest(this, ***REMOVED***
            url: this.action,
            type: this.method || "GET",
            data: clickInfo.concat($(this).serializeArray())
    ***REMOVED***);
***REMOVED***);
***REMOVED***(jQuery));
