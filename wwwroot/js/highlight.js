const links = document.querySelectorAll(".post-link");

async function highlight(linkId) {
    let postId = "";

    for (let i = 10; i < linkId.length; i++) {
        postId += linkId[i];
    }

    let post = document.getElementById("post-".concat(postId));

    post.style.animationPlayState = "running";
    await sleep(2000);
    post.style.animationPlayState = "paused";
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

links.forEach(item => item.addEventListener('click',
    () => highlight(item.id)));

