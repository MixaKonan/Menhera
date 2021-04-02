const postTexts = document.querySelectorAll(".comment");


function formatComments(posts) {
    for(let i = 0; i < posts.length; i++) {
        posts[i].innerHTML.replace(/[>>(.*)]/g, "<a href=/Thread/Thread/\"$1\">");
    }
}

formatComments(postTexts);