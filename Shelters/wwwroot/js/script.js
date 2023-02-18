$(document).ready(function() {
    $(".info__menu").mousemove(function() {
        if ($(this).hasClass('active'))
        {
            console.log(true)
        }
    })
})

const arrayMenu = [];

function InfoMenu(event){
    console.log()
    $(".info__menu").not($(event.target).parents('.info').find(".info__menu")[0]).removeClass("active");
    $(event.target).parents('.info').find(".info__menu").toggleClass('active');
}