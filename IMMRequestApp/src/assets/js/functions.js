$(document).ready(function(){ 
    $(function() {         
       //----- CLOSE
       $('[data-popup-close]').on('click', function(e)  {
           var targeted_popup_class = jQuery(this).attr('data-popup-close');
           $('[data-popup="' + targeted_popup_class + '"]').fadeOut(350);
           e.preventDefault();
       });
   });     
});

function Validate(output)
{
    var ok = true;
    var campos = $("input");
    for (var i=0; i<campos.length; i++){
        if (campos[i].value == "" && campos[i].name != "submitButton") {
            ok = false;
        }
    }
    if(ok){
        $('#' + output).html("");
        if(!ValidateMail("#mail")){
            $('.modal-title').html("ATENCION");
            $('#popup-body').html('<p style="text-align:center;">Dirección de correo inválida</p>');
            $('#popup').fadeIn(350);
            ok = false;
        }
    }else{
        $('.modal-title').html("ATENCION");
        $('#popup-body').html('<p style="text-align:center;">Todos los campos deben ser completados</p>');
        $('#popup').fadeIn(350); 
    }
    return ok;
}


function ValidateMail(campo) {
    valor = $(campo).val();
    esValido = false;
    if (valor.indexOf('@') >= 1) {
        m_valido_dom = valor.substr(valor.indexOf('@')+1);
        if (m_valido_dom.indexOf('@') == -1) {
            if (m_valido_dom.indexOf('.') >= 1) {
                m_valido_dom_e = m_valido_dom.substr(m_valido_dom.indexOf('.')+1);
                if (m_valido_dom_e.length >= 1) {
                    esValido = true;
                }
            }
        }
    }
    return esValido;
}

function clearFields(){
    $("#area").val("");
    $("#topic").val("");
    $("#type").val("");
    $("#name").val("");
    $("#email").val("");
    $("#phone").val("");
    $("#details").val("");
}

function sendRequest(){
    clearFields();
        $('.modal-title').html("Error");
        $('#popup-body').html('<p style="text-align:center;">Error al ingresar la solicitud</p>');
        $('#popup').fadeIn(350);
}