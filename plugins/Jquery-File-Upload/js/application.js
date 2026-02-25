/* 
 * jQuery File Upload Plugin JS Example 5.0.2
 * https://github.com/blueimp/jQuery-File-Upload
 *
 * Copyright 2010, Sebastian Tschan
 * https://blueimp.net
 *
 * Licensed under the MIT license:
 * http://creativecommons.org/licenses/MIT/
 */

/*jslint nomen: true */
/*global $ */

$(function () {
    'use strict';
    $('#fileupload').fileupload({
        disableImageResize: false,
        autoUpload: false,
        //disableImageResize: /Android(?!.*Chrome)|Opera/.test(window.navigator.userAgent),
        maxFileSize: 5000000,
        acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
        // Uncomment the following to send cross-domain cookies:
        //xhrFields: {withCredentials: true},                
    });

    // Enable iframe cross-domain access via redirect option:
    $('#fileupload').fileupload(
        'option',
        'redirect',
        window.location.href.replace(
            /\/[^\/]*$/,
            '/cors/result.html?%s'
        )
    );

    // Upload server status check for browsers with CORS support:
    if ($.support.cors) {
        $.ajax({
            type: 'HEAD'
        }).fail(function () {
            $('<div class="alert alert-danger"/>')
                .text('Upload server currently unavailable - ' +
                        new Date())
                .appendTo('#fileupload');
        });
    }

    // Load & display existing files:
   // $('#fileupload').addClass('fileupload-processing');
    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({
        type: "POST",
        url: "Handler.ashx",
        dataType: "json",
        //done: function (e, data) {
        //    if (data.jqXHR.responseText || data.result) {
        //        var fu = $('#fileupload').data('fileupload');
        //        var JSONjQueryObject = (data.jqXHR.responseText) ? jQuery.parseJSON(data.jqXHR.responseText) : data.result;
        //        alert(JSONjQueryObject);
        //      //  $(this)._renderDownload(JSONjQueryObject.List).appendTo($('#fileupload .files'));
        //    }
        //},
      
    });
   
   

    // Load existing files:
    //    $.getJSON($('#fileupload form').prop('action'), function (files) {
    //        var fu = $('#fileupload').data('fileupload');
    //        fu._adjustMaxNumberOfFiles(-files.length);
    //        fu._renderDownload(files)
    //            .appendTo($('#fileupload .files'))
    //            .fadeIn(function () {
    //                // Fix for IE7 and lower:
    //                $(this).show();
    //            });
    //    });
    
    //$('#fileupload').bind('fileuploaddone', function (e, data) {
    //    if (data.jqXHR.responseText || data.result) {
    //        var fu = $('#fileupload').data('fileupload');
    //        var JSONjQueryObject = (data.jqXHR.responseText) ? jQuery.parseJSON(data.jqXHR.responseText) : data.result;
            
    //        // Don't allow to add another file for each successfull File uploaded
    //        //fu._adjustMaxNumberOfFiles(JSONjQueryObject.List.length);
            
    //        //.files.length);
    //        //                debugger;
    //        fu._renderDownload(JSONjQueryObject.List)
    //            .appendTo($('#fileupload .files'))
    //            .fadeIn(function () {
    //                // Fix for IE7 and lower:
    //                $(this).show();
    //            });
    //    }
    //});

    // Open download dialogs via iframes,
    // to prevent aborting current uploads:
    //$('#fileupload .files a:not([target^=_blank])').live('click', function (e) {
    //    e.preventDefault();
    //    $('<iframe style="display:none;"></iframe>')
    //        .prop('src', this.href)
    //        .appendTo('body');
    //});

});