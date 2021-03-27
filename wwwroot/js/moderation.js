const deleteButton = document.getElementById("deletePost");
const banButton = document.getElementById("banAnon");
const deleteInput = document.getElementById("deleteInput");
const banIpInput = document.getElementById("banIpInput");

let deletionData = {postId: deleteInput.value};
let banData = {anonIpHash: banIpInput.value};

deleteButton.onclick = DeletePost;
banButton.onclick = BanAnon;


function DeletePost()
{
    $.ajax(
        {
            url:'/Admin/DeletePost',
            contentType: 'application/x-www-form-urlencoded',
            data: deletionData,
            type: 'post'
        }).done(
            () => alert('Удалено')
    );
}

function BanAnon()
{
    $.ajax(
        {
            url:'/Admin/BanAnon',
            contentType: 'application/x-www-form-urlencoded',
            data: banData,
            method: 'Post'
        }).done(
            () => alert('Забанен')
    );
}