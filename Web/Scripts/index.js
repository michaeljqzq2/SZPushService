$(document).ready(function () {
    if ($("#filter").prop("selectedIndex") === 0) {
        $("#filter").prop("selectedIndex", -1);
    }
    $("#filter").change(function() {
        $("#filter").parents("form").submit();
    });
});