     
// HEADER ---> MENU BAR
let searchBtn = document.querySelector('.searchBtn');
let closeBtn = document.querySelector('.closeBtn');
let searchBox = document.querySelector('.searchBox');
// search - button
searchBtn.onclick = function(){
    searchBox.classList.add('active');
    closeBtn.classList.add('active');
    searchBtn.classList.add('active');
}
// close - button
closeBtn.onclick = function(){
    searchBox.classList.remove('active');
    closeBtn.classList.remove('active');
    searchBtn.classList.remove('active');
}



//  SLIDER  
let slider = document.querySelector('#slider .list');
let items = document.querySelectorAll('#slider .list .item');
let next = document.getElementById('next');
let prev = document.getElementById('prev');
let dots = document.querySelectorAll('#slider .dots li');

let lengthItems = items.length - 1;
let active = 0;
next.onclick = function(){
active = active + 1 <= lengthItems ? active + 1 : 0;
reloadSlider();
}
prev.onclick = function(){
active = active - 1 >= 0 ? active - 1 : lengthItems;
reloadSlider();
}
let refreshInterval = setInterval(()=> {next.click()}, 3000);
function reloadSlider(){
slider.style.left = -items[active].offsetLeft + 'px';
// 
let last_active_dot = document.querySelector('#slider .dots li.active');
last_active_dot.classList.remove('active');
dots[active].classList.add('active');

clearInterval(refreshInterval);
refreshInterval = setInterval(()=> {next.click()}, 3000);


}

dots.forEach((li, key) => {
li.addEventListener('click', ()=>{
   active = key;
   reloadSlider();
})
})
window.onresize = function(event) {
reloadSlider();
};

/*=============== SWIPER JS ===============*/
let swiperCards = new Swiper(".card__content", {
loop: true,
spaceBetween: 32,
grabCursor: true,

pagination: {
el: ".swiper-pagination",
clickable: true,
dynamicBullets: true,
},

navigation: {
nextEl: ".swiper-button-next",
prevEl: ".swiper-button-prev",
},

breakpoints:{
600: {
  slidesPerView: 2,
},
968: {
  slidesPerView: 3,
},
},
});