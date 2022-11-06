jshelper = {};
jshelper.showAlert = function (success, msg) {
    if (success) {
        this.success(msg);
    } else {
        this.error(msg);
    }
};
jshelper.success = function (msg, callback, time) {
    toastr.success(msg, 'Información', { onHidden: callback, timeOut: time });
};
jshelper.error = function (msg) {
    toastr.error(msg, 'Alerta de Error');
};
jshelper.failure = function () {   
    toastr.error('Ago Salio mal, Por favor intente nuevamente, si el problema persiste comuniquese con el administrador del sistema.', 'Alert de Error');
};
jshelper.get = function (url, data, callback) {    
    $.ajax({
        url: url,
        data: data,
        type: 'GET',
        //processData: false, // Procesa datos de entrada
        contentType: false,
        success: callback,
        async: false
    });
};
jshelper.save = function (url, form, callback, btn) {
    var formArray = $(form).serializeArray();
    let formData = new FormData();
    for (var i of formArray) {
        if (i.name == "Principal" || i.name == "EsPorcentaje" || i.name == "AplicarDefault")
            formData.append(i.name, i.value == "on" ? true : false)
        else
            formData.append(i.name, i.value)
    }
    if (btn != null) $(btn).prop('disabled', true)
    $.ajax({
        url: url,
        type: 'POST',
        processData: false,
        contentType: false,
        data: formData,
        success: callback,
        error: function (xhr, resp, text) {
            if (btn != null) $(btn).prop('disabled', true)
            Swal.fire("Error!", xhr.responseText, "error");
        }
    });
};
jshelper.deleteConfirm = function (callback) { // , url, data){
   // alert('confirmando ajaja');
    Swal.fire({
        title: '¿ Confirmar de eliminación ?',
        text: "Esta acción es irreversible!",
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si',
        cancelButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {
            callback();//rowDelete(obj);
        }
    });
};
jshelper.deleteAjax = function (url, data, callback) {
    $.ajax({
        url: url,
        type: 'POST',
        data: jshelper.addAntiForgeryToken(data),
        success: function (response) {
            if (response.success) {
                jshelper.success(response.message)
                callback();
            } else {
                jshelper.error(response.message);
            }            
        },
        error: function (error) {
            jshelper.failure();
        }
    });
}
jshelper.deleteRedirect = function (href) {
    Swal.fire({
        title: 'Se ha eliminado  satisfactoriamente!',
        text: 'Esta ventana se cierra en 5 segundos.',
        timer: 5000,
        timerProgressBar: true,
        showCloseButton: true
    }).then(function () {
        $(location).attr('href', href);
    })
}
jshelper.addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $("[name='__RequestVerificationToken']").val();
    return data;
};
jshelper.ACRenderItem = function (ul, item) {
    var item = $('<div class="list_item_container"><div class="label"><strong>' + item.value + '</strong></div><div class="description" style="font-size: smaller;">' + item.desc + '</div></div>')
    return $("<li>").append(item).appendTo(ul);
}
