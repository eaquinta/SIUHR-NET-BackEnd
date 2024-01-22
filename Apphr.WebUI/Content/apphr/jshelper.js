/* v1.0 */
jshelper = {};
jshelper.debug = false;
jshelper.bindFormData = function (campo, formularioOrigen, formularioDestino) {
    $('#' + formularioDestino + ' #' + campo).val($('#' + formularioOrigen + ' #' + campo).val());
}
jshelper.bindValidation = function (forma) {
    //console.log(forma);
    forma
        .removeData('validator')
        .removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(forma);
}
jshelper.showAlert = function (success, msg) {
    if (success) {
        this.success(msg);
    } else {
        this.error(msg);
    }
};
jshelper.momentDate = function (d) {
    return moment(d).format('yyyy-MM-DD');
}
jshelper.success = function (msg, callback, time) {
    toastr.success(msg, 'Información', { onHidden: callback, timeOut: time });
};
jshelper.error = function (msg) {
    toastr.error(msg, 'Alerta de Error');
};
jshelper.failure = function () {   
    toastr.error('Ago Salio mal, Por favor intente nuevamente, si el problema persiste comuniquese con el administrador del sistema.', 'Alert de Error');
};
jshelper.showPermissions = function () {
    if (permissions != null) {
        if (!permissions.View) {
            $('#icon-view').hide();
        }
        if (!permissions.Create) {
            $('#icon-create').hide();
            $('#btn-add-child').hide();
        }
        if (!permissions.Edit) {
            $('#icon-edit').hide();
            $('#btnSaveMaster').hide();
        }
        if (!permissions.Delete) {
            $('#icon-delete').hide();
        }
    }
}
//jshelper.IsJsonString = function (s) {
//    try {
//        var obj = JSON.parse(s);
//        // More strict checking     
//        // if (obj && typeof obj === "object") {
//        //    return true;
//        // }
//        return true;
//    } catch (e) {
//        return false;
//    }
//    return true;
//}
jshelper.cleanResponse = function (r) {
    if (r.indexOf('<!DOCTYPE html>') != -1) {
        r = $(r).filter('.container').html();
        r = $(r).removeClass('vh-100');
        r = $(r).find('#btn-go-home').remove().end();
    }
    return r;
}
jshelper.get = function (url, data, callback) {    
    $.ajax({
        url: url,
        data: data,
        type: 'GET',
        //processData: false, // Procesa datos de entrada
        contentType: false,
        success: callback,
        async: false,
        error: function () { jshelper.failure(); }
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

jshelper.deleteConfirm = function (callback, message) {
    message = typeof message !== 'undefined' ? message : 'Esta acción es irreversible !';
    Swal.fire({
        title: '¿Está seguro que desea eliminar?',
        //text: message,
        html: message,
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si',
        cancelButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {
            callback();
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
    var item = $('<div class="list_item_container"><div class="label"><strong>' + item.value + '</strong></div><div class="description" style="font-size: smaller;">' + item.desc + '</div></div>');
    return $("<li>").append(item).appendTo(ul);
}
jshelper.ACRenderItemLbl = function (ul, item) {    
    var item = $('<div class="list_item_container"><div class="label"><strong>' + item.label+ '</strong></div><div class="description" style="font-size: smaller;">' + item.desc + '</div></div>');
    return $("<li>").append(item).appendTo(ul);
}
