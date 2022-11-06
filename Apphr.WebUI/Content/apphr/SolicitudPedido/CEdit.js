
var master = {};
var child = {};

function refreshGrid() {    
    if (!master.isInsert()) {
        jshelper.get(urlGrid, master.idData(), function (data) {            
            $('#Grid1').html(data);
        });
    }
}


$(function () {

    child.btnSave = $('#btnSaveChild');
    child.modal = $('#child-modal');
    child.rowCreate = function () {
        child.initForm();
        child.clearForm();
    };
    child.rowDelete = function (obj) {
        jshelper.deleteConfirm(function () {
            //rowDelete(obj);
            id = $(obj).data('model-id');
            jshelper.deleteAjax(
                urlDeleteChild,
                { id: id },
                function () {
                    refreshGrid();
                }
            );
        });
    };
    child.rowEdit = function (obj) {
        var id = $(obj).data("model-id");       
        jshelper.get(urlGetChild, { id: id }, function (response) {            
            if (response.success) {
                child.initForm();
                child.fillForm(response.data);
                child.modal.modal('show');
            } else {
                jshelper.error(response.message);
            }
        });
    };
    child.saveBegin = function () {
        child.btnSave.prop('disabled', true);
    };
    child.saveEnd = function () {
        child.btnSave.prop('disabled', false);
        child.modal.modal('hide');
    };
    child.saveSuccess = function (response) {        
        //console.log(response);
        jshelper.showAlert(response.success, response.message);
        if (response.success) {}
        child.saveEnd();
        refreshGrid();
    };
    child.saveFailure = function () {
        jshelper.failure();
        child.saveEnd();
    };
    child.clearForm = function () {
        $('#SolicitudPedidoDTId').val('0');
        $('#Cantidad').val('');
        $('#Precio').val('');
        $('#Valor').val('');
        $('#MaterialPrecio').val('');
        $('#UnidadMedida').val('');
        $('#MaterialNombre').val('');
        $('#MaterialId').val('');
        $('#MaterialCodigo').val('');
        // Clear Validation errors
        $('.field-validation-error').html("");
    };
    child.fillForm = function (data) {
        //console.log(data);
        //$('#formLinea > #mode').val('UPD');
        //$('#SolicitudPedidoId').val(id1);
        $('#SolicitudPedidoDTId').val(data.SolicitudPedidoDTId);        
        $('#MaterialId').val(data.MaterialId);        
        $('#MaterialCodigo').val(data.Material.Codigo);
        $('#MaterialNombre').val(data.Material.Descripcion);
        $('#MaterialPrecio').val(data.Material.Precio);
        $('#UnidadMedida').val(data.Material.UnidadMedida);
        $('#Cantidad').val(data.Cantidad);
        $('#Precio').val(data.Precio);
        $('#Valor').val(data.Valor);
    };
    child.initForm = function () {
        $('#MaterialNombre, #MaterialPrecio, #UnidadMedida, #Valor').attr('readonly', 'readonly');        
        $('#child-form #SolicitudDespachoId').val(id1);
    };

    master.id1 = $('#SolicitudPedidoId');
    master.btnSave = $('#btnSaveMaster');
    master.idData = function () { return { id: master.id1.val() } };
    master.saveBegin = function () {
        master.btnSave.prop('disabled', true);
    }
    master.saveSuccess = function (response) {        
        if (response.success) {
            if (master.isInsert()) {
                master.id1.val(response.data.SolicitudPedidoId);
                jshelper.success(response.message + '<br/>En un momento será redireccionado', function () {
                    window.location.replace(urlCEdit + '/' + master.id1.val());
                }, 1000)
            } else {
                jshelper.success(response.message);
                master.initForm();
                refreshGrid();
            }            
        } else {
            jshelper.error(response.message);
        }
        master.saveEnd();
    }
    master.saveFailure = function () {
        jshelper.failure();
        master.saveEnd();
    }
    master.saveEnd = function () {
        master.btnSave.prop('disabled', false);
    }
    master.isInsert = function () {
        return master.id1.val() == '';
    };
    master.clearForm = function () {
        $("#Correlativo").val('');
        $("#Fecha").val('');
        $("#Tipo").val('');
        $("#DepartamentoId").val('');
        $("#DepartamentoSeccionId").val('');
        $("#Elaboro").val('');
        $("#Solicito").val('');
        $("#JefeDepartamento").val('');
        $("#Gerente").val('');
        $("#Director").val('');
        $("#Observaciones").val('');
    };
    master.delete = function () {

    };
    master.initForm = function () {
        $('#DepartamentoNombre, #SeccionNombre, #Correlativo').attr('readonly', 'readonly');
        if (master.isInsert()) {
            master.btnSave.html('<i class="fa fa-plus"></i> Agregar').removeClass('btn-warning').addClass('btn-success');
            $('.btnReturn').attr("href", urlIndex);
            $('.onIns').show();
            $('.onUpd').hide();
        }
        else {
            master.btnSave.html('<i class="fas fa-edit"></i> Actualizar').removeClass('btn-success').addClass('btn-warning');
            $('.btnReturn').attr("href", urlDetails + '/'+ master.id1.val())
            $('.onIns').hide();
            $('.onUpd').show();
            $('#SolicitudDespachoId').attr('readonly', 'readonly');
        }
    }


    //init form
    master.initForm();
    refreshGrid();

    //bind functions
    master.btnSave.click(function () {
        if (!master.isInsert) {
            Swal.fire({
                title: '¿ Desea de actualizar ?',
                text: "Esta acción es irreversible!",
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Si',
                cancelButtonText: 'No'
            }).then((result) => {
                if (result.isConfirmed) {
                    $("#master-form").submit();
                }
            });
        } else {
            $("#master-form").submit();
        }
    });
    $('#DepartamentoCodigo').on('blur', function () {
        $.ajax({
            type: "GET",
            url: urlGetDestinoName,
            data: { id: $("#DepartamentoCodigo").val() },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                //console.log(response);
                if (response.success) {
                    $('#DepartamentoNombre').val(response.data.Descripcion);
                    $('#DepartamentoId').val(response.data.DestinoId);
                    $('#JefeDepartamento').val(response.data.JefeServicio);
                }
            },
            failure: function (response) {
                console.log(response);
            }
        });
    });
    $('#SeccionCodigo').on('change', function () {
        $.ajax({
            type: "GET",
            url: urlGetDestinoName,//  this for calling the web method function in controller.
            data: { id: $("#SeccionCodigo").val() }, // user name value
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                //console.log(response);
                if (response.success) {
                    $('#SeccionNombre').val(response.data.Descripcion);
                    $('#SeccionId').val(response.data.DestinoId);
                }
            },
            failure: function (response) {
                console.log(response);
            }
        });
    });
    $('.btn-eliminar').click(function (event) {
        event.preventDefault();
        jshelper.deleteConfirm(function () {
            jshelper.deleteAjax(
                urlDeleteMaster,
                master.idData(),
                function () { jshelper.deleteRedirect(urlIndex); }
            );
        });
    });
    $('#Cantidad ,#Precio').on('change', function () {
        var v = $('#Cantidad').val() * $('#Precio').val();
        $('#Valor').val(v);
    });
    $('#MaterialCodigo').on('change', function () {
        jshelper.get(
            urlGetMatByCod,
            { id: $("#MaterialCodigo").val() },
            function (response) {
                if (response.success) {
                    //console.log(response.data);
                    if (response.data != null) {
                        $('#MaterialId').val(response.data.MaterialId);
                        $('#MaterialNombre').val(response.data.Descripcion);
                        $('#UnidadMedida').val(response.data.UnidadMedida);
                        $('#MaterialPrecio').val(response.data.Precio);
                        $('#Cantidad').focus();
                    } else {
                        $('#MaterialId').val('');
                        $('#MaterialNombre').val('');
                        $('#UnidadMedida').val('');
                        $('#MaterialPrecio').val('');
                    }
                }
            }
        );        
    });
    $("#MaterialCodigo").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlFindMateriales,
                dataType: "json",
                data: "id=" + request.term,
                success: function (resp) {
                    response($.map(resp.data, function (item) {
                        //console.log(item);
                        return {
                            label: item.id,
                            value: item.id,
                            desc: item.text
                        };
                    }));
                }
            });
        },
        appendTo: "#formLinea"
    }).data("ui-autocomplete")._renderItem = function (ul, item) {
        var item = $('<div class="list_item_container"><div class="label"><strong>' + item.label + '</strong></div><div class="description" style="font-size: smaller;">' + item.desc + '</div></div>')
        return $("<li>").append(item).appendTo(ul);
    };
    $("#DepartamentoCodigo, #SeccionCodigo").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlFindDestinos,
                dataType: "json",
                data: "filter=" + request.term,
                success: function (resp) {
                    response($.map(resp.data, function (item) {
                        //console.log(item);
                        return {
                            label: item.id,
                            value: item.id,
                            desc: item.text
                        };
                    }));
                }
            });
        }
    }).each(function (idx, ele) {
        $(ele).data("ui-autocomplete")._renderItem = function (ul, item) {
            var item = $('<div class="list_item_container"><div class="label"><strong>' + item.label + '</strong></div><div class="description" style="font-size: smaller;">' + item.desc + '</div></div>')
            return $("<li>").append(item).appendTo(ul);
        }
    });
  
    
});
