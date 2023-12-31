
//Scrolling header
window.addEventListener("scroll", function() {
    const div = document.querySelector("#header");
    if (window.scrollY > 100) { // Adjust this value to change when the color change occurs
        div.classList.add("scrolled");
    } else {
        div.classList.remove("scrolled");
    }
});

//count number
const counts = document.querySelectorAll(".counter-numbers");
counts.forEach((count, index) => {
    const target = parseInt(count.innerText);
    const step = target / 500; // Adjust the step value as needed
    let current = 0;

    const updateCount = () => {
        current += step;
        if (current < target) {
            count.innerText = Math.round(current);
            requestAnimationFrame(updateCount);
        } else {
            count.innerText = target;
        }
    };

    updateCount();
});
// content 3 - count
const count = document.querySelectorAll(".counts");
count.forEach((count, index) => {
    const target = parseInt(count.innerText);
    const step = target / 500; // Adjust the step value as needed
    let current = 0;

    const updateCount = () => {
        current += step;
        if (current < target) {
            count.innerText = Math.round(current);
            requestAnimationFrame(updateCount);
        } else {
            count.innerText = target;
        }
    };

    updateCount();
});


//swiper
var swiper = new Swiper(".mySwiper", {
  slidesPerView: 1,
  spaceBetween: 180,
  loop: true,
  centerSlide: 'true',
  fade: 'true',   
  grabCursor: 'true',

  autoplay: {
  delay: 2500,
  disableOnInteraction: false,
  
  },
  speed: 1000,
  pagination: {
    el: ".swiper-pagination",
    clickable: true,
    dynamicBullets: true,
  },
  navigation: {
    nextEl: ".swiper-button-next",
    prevEl: ".swiper-button-prev"
  }
});

// Paganation
let thisPage = 1;
let limit = 16;
let list = document.querySelectorAll('.all-posts .post-item');

function loadItem(){
    let beginGet = limit * (thisPage - 1);
    let endGet = limit * thisPage - 1;
    list.forEach((item, key)=>{
        if(key >= beginGet && key <= endGet){
            item.style.display = 'block';
        }else{
            item.style.display = 'none';
        }
    })
    listPage();
}
loadItem();
function listPage(){
    let count = Math.ceil(list.length / limit);
    document.querySelector('.listPage').innerHTML = '';

    if(thisPage != 1){
        let prev = document.createElement('li');
        prev.innerText = 'PREV';
        prev.setAttribute('onclick', "changePage(" + (thisPage - 1) + ")");
        document.querySelector('.listPage').appendChild(prev);
    }

    for(i = 1; i <= count; i++){
        let newPage = document.createElement('li');
        newPage.innerText = i;
        if(i == thisPage){
            newPage.classList.add('active');
        }
        newPage.setAttribute('onclick', "changePage(" + i + ")");
        document.querySelector('.listPage').appendChild(newPage);
    }

    if(thisPage != count){
        let next = document.createElement('li');
        next.innerText = 'NEXT';
        next.setAttribute('onclick', "changePage(" + (thisPage + 1) + ")");
        document.querySelector('.listPage').appendChild(next);
    }
}
function changePage(i){
    thisPage = i;
    loadItem();
}