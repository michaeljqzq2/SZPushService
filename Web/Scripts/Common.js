function setReferer() {
    //delete window.document.referrer;
    //window.document.__defineGetter__('referrer', function () {
    //    return "smzdm.com";
    //});
    //Object.defineProperty(document, "referrer", { get: function () { return "www.smzdm.com"; } });

    window.img_array = new Array();
    $("img:not([src*='manmanbuy'])").each( function (i, val) {
        var ifr = create_img_iframe($(val).attr("src"), i);
        $(val).after(ifr);
        $(val).remove();
    });
}

function create_img_iframe(url,count) {
    var frameid = 'frameimg' + Math.random();
    window.img = '<img id="img" src=\'' + url + '?' + Math.random() + '\' /><script>window.onload = function() { parent.document.getElementById(\'' + frameid + '\').height = document.getElementById(\'img\').height+\'px\'; }<' + '/script>';
    window.img_array[count] = window.img;
    ifr = document.createElement('iframe');
    ifr.src = "javascript:parent.img_array[" + count + "];";
    ifr.frameBorder = "0";
    ifr.scrolling = "no";
    ifr.width = "100%";
    ifr.id = frameid;

    return ifr;
}