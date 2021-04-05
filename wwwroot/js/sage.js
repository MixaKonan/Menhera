const checkbox =  document.getElementById("sage");
const email =  document.getElementById("email");

function SageThread()
{
    if(checkbox.checked)
    {
        email.value = "sage";
    }
    if(!checkbox.checked)
    {
        email.value = "";
    }
}

checkbox.onchange = SageThread;