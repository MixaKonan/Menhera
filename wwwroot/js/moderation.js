document.querySelectorAll(".deletePost").forEach(item =>
    item.addEventListener('click', event => DeletePost(item.id)))

function DeletePost(stringId)
{
    let id = "";

    for(let i = 11; i < stringId.length; i++)
    {
        id += stringId[i];
    }

    let deletionData = {postId: document.getElementById("deleteInput-".concat(id)).value};

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
    let banData = {anonIpHash: banIpInput.value};

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