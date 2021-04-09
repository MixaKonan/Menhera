const replyButtons = document.querySelectorAll(".reply-link");
const form = document.querySelector("form");

replyButtons.forEach(item => item.addEventListener('click',
    () => moveForm(item.id)));

function moveForm(linkId) {
    let id = "";
    
    for(let i = 3; i < linkId.length; i++) {
        id += linkId[i];
    }

    let email = form.querySelector('input[name="email"]');
    email.style.display = 'none';
    let submit = form.querySelector('input[type="submit"]');
    submit.className = 'field';
    let name = form.querySelector('input[name="anonname"]');

    if (name) {
        name.style.display = 'none';
    }

    let subject = form.querySelector('input[name="subject"]');

    if (subject) {
        subject.style.display = 'none';
    }


    let article = document.getElementById("post-".concat(id))
    let comment = form.querySelector('textarea[name="Comment"]');
    comment.style.height = '100px';
    comment.className = 'field';
    article.appendChild(form);
    form.scrollIntoView();
    comment.focus();
    comment.value += '>>' + id + '\n';
}