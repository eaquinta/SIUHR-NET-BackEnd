﻿
var master = {};
var child = {};




$(function () {

    master.id1 = $('#IngresoInventarioId');
    master.btnSave = $('#btnSaveMaster');
    master.grid = $('#Grid1');
    master.idData = () => { return { id: master.id1.val() } };
    master.saveBegin =  () =>  {
        master.btnSave.prop('disabled', true);
    }
    master.saveSuccess = (response) => {
        if (response.success) {
            if (master.isInsert()) {
                master.id1.val(response.data.IngresoInventarioId);
                jshelper.success(response.message + '<br/>En un momento será redireccionado', function () {
                    window.location.replace(urlCEdit + '/' + master.id1.val());
                }, 1000)
            } else {
                jshelper.success(response.message);
                master.initForm();
                master.refreshGrid();
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
        $('#IngresoInventarioId, #BodegaDescripcion, #SeccionNombre, #Correlativo').attr('readonly', 'readonly').attr('tabindex','-1');
        if (master.isInsert()) {
            master.btnSave.html('<i class="fa fa-plus"></i> Agregar').removeClass('btn-warning').addClass('btn-success');
            $('.btnReturn').attr("href", urlIndex);
            $('.onIns').show();
            $('.onUpd').hide();
        }
        else {
            master.btnSave.html('<i class="fas fa-edit"></i> Actualizar').removeClass('btn-success').addClass('btn-warning');
            $('.btnReturn').attr("href", urlDetails + '/' + master.id1.val())
            $('.onIns').hide();
            $('.onUpd').show();
            $('#SolicitudDespachoId').attr('readonly', 'readonly');
        }
    }
    master.refreshGrid = () => {
        if (!master.isInsert()) {
            master.grid.fadeTo(0, 0.4);
            jshelper.get(urlGrid, master.idData(), function (data) {
                $('#Grid1').html(data);
                master.grid.fadeTo('slow', 1);
            });
        }
    }

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
                    master.refreshGrid();
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
    };
    child.saveSuccess = function (response) {        
        //console.log(response);
        jshelper.showAlert(response.success, response.message);
        if (response.success) {
            child.modal.modal('hide');
        }
        child.saveEnd();
        master.refreshGrid();
    };
    child.saveFailure = function () {
        jshelper.failure();
        child.saveEnd();
    };
    child.clearForm = function () {
        // Hiddens
        $('#form-child #BodegaId').val($('#BodegaId').val());
        $('#form-child #Fecha').val($('#Fecha').val());
        $('#IngresoInventarioDetalleId').val('0');
        $('#ProveedorId').val('');
        $('#MaterialId').val('');
        // Showing
        $('#Cantidad').val('');
        $('#Precio').val('');
        $('#Valor').val('');
        $('#MaterialPrecio').val('');
        $('#MaterialUM').val('');
        $('#MaterialNombre').val('');
        $('#MaterialCodigo').val('');
        $('#ProveedorNit').val('');
        $('#ProveedorNombre').val('');
        $('#Observacion').val('');
        $('#FechaVencimiento').val('');
        $('#Lote').val('');
        // Clear Validation errors
        $('.field-validation-error').html("");
    };
    child.fillForm = function (data) {
        //console.log(data);
        // Hiddens
        $('#form-child #BodegaId').val($('#BodegaId').val());
        $('#form-child #Fecha').val($('#Fecha').val());
        $('#IngresoInventarioDetalleId').val(data.IngresoInventarioDetalleId);        
        $('#ProveedorId').val(data.ProveedorId);
        $('#MaterialId').val(data.MaterialId);
        // Showing
        $('#MaterialCodigo').val(data.Material.Codigo);
        $('#MaterialNombre').val(data.Material.Descripcion);
        $('#MaterialPrecio').val(data.Material.Precio);
        $('#MaterialUM').val(data.Material.UnidadMedida);
        $('#Cantidad').val(data.Cantidad);
        $('#Precio').val(data.Precio);
        $('#Valor').val(data.Valor);
        $('#FechaVencimiento').val(moment(data.FechaVencimiento).format('yyyy-MM-DD'));
        $('#ProveedorNit').val(data.ProveedorNit);
        $('#ProveedorNombre').val(data.ProveedorNombre);
        $('#Observacion').val(data.Observacion);
        $('#Lote').val(data.Lote);
    };
    child.initForm = function () {
        $('#MaterialNombre, #MaterialPrecio, #MaterialUM, #Valor, #ProveedorNombre, #MaterialMinimo, #MaterialExistencia').attr('readonly', 'readonly').attr('tabindex','-1');
        $('#form-child #DocumentoReferencia').val($('#DocumentoReferencia').val());
    };

    


    //init form
    master.initForm();
    master.refreshGrid();

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
    $('#BodegaNombre').on('blur', function () {
        $.ajax({
            type: "GET",
            url: urlGetBodega,
            data: { nombre: $("#BodegaNombre").val() },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log(response);
                if (response.success) {
                    //$('#DepartamentoNombre').val(response.data.Descripcion);
                    $('#BodegaId').val(response.data.BodegaId);
                    $('#BodegaDescripcion').val(response.data.Descripcion);
                }
            },
            failure: function (response) {
                console.log(response);
                jshelper.failure();
            }
        });
    });
    $('#ProveedorNit').on('blur', function () {
        $.ajax({
            type: "GET",
            url: urlGetProveedor,
            data: { val: $("#ProveedorNit").val() },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                //console.log(response);
                if (response.success) {
                    $('#ProveedorNombre').val(response.data.Descripcion);
                    $('#ProveedorId').val(response.data.ProveedorId);
                }
                else {
                    $('#ProveedorNombre').val('');
                    $('#ProveedorId').val('');
                }
            },
            failure: function (response) {
                console.log(response);
                jshelper.failure();
            }
        });
    });  
    $('#Cantidad ,#Precio').on('blur', function () {
        var v = $('#Cantidad').val() * $('#Precio').val();
        $('#Valor').val(v);
    });
    $('#MaterialCodigo').on('blur', function () {
        jshelper.get(
            urlGetMatByCod,
            { id: $("#MaterialCodigo").val(), BodegaId: $('#BodegaId').val() },
            function (response) {
                if (response.success) {
                    //console.log(response.data);
                    if (response.data != null) {
                        $('#MaterialId').val(response.data.MaterialId);
                        $('#MaterialNombre').val(response.data.Descripcion);
                        $('#MaterialUM').val(response.data.UnidadMedida);
                        $('#MaterialPrecio').val(response.data.Precio);
                        $('#MaterialMinimo').val(response.data.Minimo);
                        $('#MaterialExistencia').val(response.data.Existencia);
                        $('#Cantidad').focus();
                    } else {
                        $('#MaterialId').val('');
                        $('#MaterialNombre').val('');
                        $('#MaterialUM').val('');
                        $('#MaterialPrecio').val('');
                        $('#MaterialMinimo').val('');
                        $('#MaterialExistencia').val('');
                    }
                }
            }
        );        
    });
    //Autocomplete
    $("#MaterialCodigo").autocomplete({
        maxShowItems: 5,
        source: function (request, response) {
            $.ajax({
                url: urlFindMateriales,
                dataType: "json",
                data: "f=" + request.term,
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
        appendTo: "#form-body-child"
    }).data("ui-autocomplete")._renderItem = function (ul, item) {
        var item = $('<div class="list_item_container"><div class="label"><strong>' + item.label + '</strong></div><div class="description" style="font-size: smaller;">' + item.desc + '</div></div>')
        return $("<li>").append(item).appendTo(ul);
        };
    $("#ProveedorNit").autocomplete({
        maxShowItems: 5,
        source: function (request, response) {
            $.ajax({
                url: urlFindProveedor,
                dataType: "json",
                data: "val=" + request.term,
                success: function (resp) {
                    response($.map(resp.data, function (item) {
                        //console.log(item);
                        return {
                            label: item.Value,
                            value: item.Value,
                            desc: item.Text
                        };
                    }));
                }
            });
        },
        appendTo: "#form-body-child"
    }).data("ui-autocomplete")._renderItem = function (ul, item) {
        var item = $('<div class="list_item_container"><div class="label"><strong>' + item.label + '</strong></div><div class="description" style="font-size: smaller;">' + item.desc + '</div></div>')
        return $("<li>").append(item).appendTo(ul);
    };
    $("#BodegaNombre").autocomplete({
        maxShowItems: 5,
        source: function (request, response) {
            $.ajax({
                url: urlFindBodegas,
                dataType: "json",
                data: "val=" + request.term,
                success: function (resp) {
                    response($.map(resp.data, function (item) {
                        //console.log(item);
                        return {
                            label: item.Text,
                            value: item.Value,
                            desc: item.Text
                        };
                    }));
                }
            });
        },
        appendTo: "#form-body-child"
    }).each(function (idx, ele) {
        $(ele).data("ui-autocomplete")._renderItem = function (ul, item) {
            var item = $('<div class="list_item_container"><div class="label"><strong>' + item.value + '</strong></div><div class="description" style="font-size: smaller;">' + item.desc + '</div></div>')
            return $("<li>").append(item).appendTo(ul);
        }
    });
  
    
});
