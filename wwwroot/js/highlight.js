const links = document.querySelectorAll(".post-link");

function Highlight(linkId) {
    let postId = "";
    
    for(let i = 10; i < linkId.length; i++) {
        postId += linkId[i];
    }
    
    let post = document.getElementById(postId);
    
    post.style.background = "rgba(249, 193, 231, 0.4)";
}

links.forEach(item => item.addEventListener('click',
    () => Highlight(item.id)));

