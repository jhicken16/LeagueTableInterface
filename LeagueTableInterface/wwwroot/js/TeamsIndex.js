let preFav = null
const cssClass = $(".addFavoriteStyle")
let favTeam = getCookie("favTeam")


preFav = favTeam;
$('#' + preFav).addClass("addFavoriteStyle")


$('select[name="favTeam"]').change(function () {
    if ($(this).val() !== 'Select Favorite' && $(this).val() !== preFav) {

        $("#" + preFav).removeClass("addFavoriteStyle")
        const elemId = $(this).val()
        $("#" + elemId).addClass("addFavoriteStyle")
        preFav = elemId

    }
});

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}



