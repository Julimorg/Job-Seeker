function moreOption(item,item2){
    console.log(`i${item}`);
    console.log(`i${item + 1}`);
    var rotating = document.querySelector(`.i${item}`);
    var background = rotating.parentElement.parentElement;
    var appear = document.querySelector(`.h${item}`);
    var appear2 = document.querySelector(`.h${item2}`);
    if(!rotating.classList.contains("rotate")){
        rotating.classList.add("rotate");
        background.classList.add(`active${item}`);
        appear.classList.add("appear");
        appear2.classList.add("appear2");
        translate.classList.add("moveDown");
    }
    else{
        rotating.classList.remove("rotate");
        background.classList.remove(`active${item}`);
        appear.classList.remove("appear");
        appear2.classList.remove("appear2");
        translate.classList.remove("moveDown");
    }        
}