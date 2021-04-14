const formTextArea = document.getElementById('comment');

function doAddTags(tag1,tag2, obj) {
    if (document.selection)
    {
        let sel = document.selection.createRange();
        sel.text = tag1 + sel.text + tag2;
    }
    else
    {
        let len = obj.value.length;
        let start = obj.selectionStart;
        let end = obj.selectionEnd;
        let scrollTop = obj.scrollTop;
        let scrollLeft = obj.scrollLeft;
        let sel = obj.value.substring(start, end);
        let rep = tag1 + sel + tag2;

        obj.value =  obj.value.substring(0,start) + rep + obj.value.substring(end,len);
        obj.scrollTop = scrollTop;
        obj.scrollLeft = scrollLeft;
        obj.focus();
        obj.setSelectionRange(start+tag1.length, end+tag1.length);
    }
}
