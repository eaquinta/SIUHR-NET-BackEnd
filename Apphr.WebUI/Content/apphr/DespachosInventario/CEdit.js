
var master = {};
var child = {};

$(function () {

    master.id1 = $('#DespachoInventarioId');
    master.btnSave = $('#btnSaveMaster');
    master.idData = function () { return { id: master.id1.val() } };
    master.saveBegin = function () {
        master.btnSave.prop('disabled', true);
    }
    master.isInsert = function () {
        return master.id1.val() == '';
    };
    master.saveSuccess = function (response) {
        if (response.success) {
            if (master.isInsert()) {                
                master.id1.val(response.data.DespachoInventarioId);
                jshelper.success(response.message + '<br/>En un momento será redireccionado', function () {
                    window.location.replace(urls.CEdit + '/' + master.id1.val());
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
    master.initForm = function () {
        $('#DespachoInventarioId, #BodegaDescripcion, #DepartamentoNombre').attr('readonly', 'readonly').attr('tabindex','-1');
        if (master.isInsert()) {
            $('#Mode').val('INS');
            master.btnSave.html('<i class="fa fa-plus"></i> Agregar').removeClass('btn-warning').addClass('btn-success');
            $('.btnReturn').attr("href", urls.Index);
            $('.onIns').show();
            $('.onUpd').hide();
        }
        else {
            $('#Mode').val('UPD');
            //$('#collapseFilter').collapse('hide');
            $('#TipoDocumentoReferencia, #DocumentoReferencia').attr('readonly', 'readonly').attr('tabindex', '-1');
            $('#TipoDocumentoReferencia').attr("disabled", true);
            master.btnSave.html('<i class="fas fa-edit"></i> Actualizar').removeClass('btn-success').addClass('btn-warning');
            $('.btnReturn').attr("href", urls.Details + '/' + master.id1.val())
            $('.onIns').hide();
            $('.onUpd').show();
            $('#SolicitudDespachoId').attr('readonly', 'readonly');
        }
    }    
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
    }
//    master.delete = function () {
//    };
    master.refreshGrid = function () {
        if (!master.isInsert()) {
            jshelper.get(urls.Grid, master.idData(), function (data) {
                $('#Grid1').html(data);
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
            id = $(obj).data('model-id');
            jshelper.deleteAjax(
                urls.DeleteChild,
                { id: id },
                function () {
                    master.refreshGrid();
                }
            );
        });
    };
    child.rowEdit = (obj) => {
        var id1 = $(obj).data("model-id");
        var id2 = $('#BodegaId').val();
        jshelper.get(urls.GetChild, { id1: id1, id2: id2 }, function (response) {
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
        $('#DespachoInventarioDetalleId').val('0');
        $('#ProveedorId').val('');
        $('#MaterialId').val('');
        $('#PacienteId').val('');
        // Showing
        $('#BodegaNombre').val('');
        $('#BodegaDescripcion').val('');
        $('#MaterialCodigo').val('');
        $('#MaterialNombre').val('');
        $('#MaterialPrecio').val('');
        $('#MaterialUM').val('');
        $('#Cantidad').val('');
        $('#MaterialExistencia').val('');
        $('#MaterialMinimo').val('');
        $('#Lote').val('');
        $('#FechaVencimiento').val('');
        $('#ProveedorNit').val('');
        $('#ProveedorNombre').val('');
        $('#PacienteRM').val('');
        $('#PacienteNombreCompleto').val('');
        $('#Observacion').val('');
        // ?
        $('#Valor').val('');
        $('#MaterialPrecio').val('');
        $('#MaterialSolicitado').val('');
        // Clear Validation errors
        $('.field-validation-error').html("");
    };
    child.fillForm = function (data) {
        //console.log(data);
        // Hiddens
        $('#form-child #BodegaId').val($('#BodegaId').val());
        $('#form-child #Fecha').val($('#Fecha').val());
        $('#DespachoInventarioDetalleId').val(data.DespachoInventarioDetalleId);
        $('#ProveedorId').val(data.ProveedorId);
        $('#MaterialId').val(data.MaterialId);
        $('#PacienteId').val(data.PacienteId);
        // Showing
        $('#MaterialCodigo').val(data.Material.Codigo);
        $('#MaterialNombre').val(data.Material.Descripcion);
        $('#MaterialPrecio').val(data.Material.Precio);
        $('#MaterialUM').val(data.Material.UnidadMedida);
        $('#MaterialExistencia').val(data.MaterialExistencia);
        $('#MaterialMinimo').val(data.MaterialMinimo);
        $('#Cantidad').val(data.Cantidad);
        $('#Lote').val(data.Lote);
        $('#FechaVencimiento').val(moment(data.FechaVencimiento).format('yyyy-MM-DD'));        
        $('#ProveedorNit').val(data.ProveedorNit);
        $('#ProveedorNombre').val(data.ProveedorNombre);
        $('#PacienteRM').val(data.PacienteRM);
        $('#PacienteNombreCompleto').val(data.PacienteNombreCompleto);
        $('#Observacion').val(data.Observacion);
        // ?
        $('#Precio').val(data.Precio);
        $('#Valor').val(data.Valor);
        $('#MaterialSolicitado').val(data.MaterialSolicitado)

    };
    child.initForm = function () {
        if ($('#TipoDocumentoReferencia').val() == 'SD') {
            $('.paciente').hide();
        }
        $('#MaterialNombre, #MaterialPrecio, #MaterialUM, #ProveedorNombre, #PacienteRM, #PacienteNombreCompleto, #MaterialExistencia, #MaterialMinimo, #MaterialSolicitado').attr('readonly', 'readonly').attr('tabindex', '-1');
        $('#child-form #DespachoInventarioId').val(master.id1.val());
        $('#form-child #BodegaId').val($('#BodegaId').val());
        $('#form-child #DestinoId').val($('#DepartamentoId').val());
        $('#form-child #DocumentoReferencia').val($('#DocumentoReferencia').val());
    };

    


    //init form
    master.initForm();
    master.refreshGrid();

//    //bind functions
//    master.btnSave.click(function () {
//        if (!master.isInsert) {
//            Swal.fire({
//                title: '¿ Desea de actualizar ?',
//                text: "Esta acción es irreversible!",
//                icon: 'question',
//                showCancelButton: true,
//                confirmButtonColor: '#3085d6',
//                cancelButtonColor: '#d33',
//                confirmButtonText: 'Si',
//                cancelButtonText: 'No'
//            }).then((result) => {
//                if (result.isConfirmed) {
//                    $("#master-form").submit();
//                }
//            });
//        } else {
//            $("#master-form").submit();
//        }
//    });
    $('.btn-eliminar').click(function (event) {
        event.preventDefault();
        jshelper.deleteConfirm(function () {
            jshelper.deleteAjax(
                urls.DeleteMaster,
                master.idData(),
                function () { jshelper.deleteRedirect(urls.Index); }
            );
        });
    });


    // Region Blur
    $('#DepartamentoCodigo').on('blur', function () {
        $.ajax({
            type: "GET",
            url: urls.GetDestino,
            data: { id: $("#DepartamentoCodigo").val() },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#DestinoId').val('');
                $('#DepartamentoNombre').val('');
                $('#JefeDepartamento').val('');
                //console.log(response);
                if (response.success) {
                    $('#DepartamentoId').val(response.data.DestinoId);
                    $('#DepartamentoNombre').val(response.data.Descripcion);                
                    $('#JefeDepartamento').val(response.data.JefeServicio);
                    $('#BodegaNombre').focus();
                }
            },
            failure: function (response) {
                console.log(response);
            }
        });
    });
    $('#BodegaNombre').on('blur', function () {
        $.ajax({
            type: "GET",
            url: urls.GetBodega,
            data: { nombre: $("#BodegaNombre").val() },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#BodegaId').val('');
                $('#BodegaDescripcion').val('');
                //console.log(response);
                if (response.success) {                   
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
    $('#MaterialCodigo').on('blur', function () {
        let DocTipo = $('#TipoDocumentoReferencia').val();
        jshelper.get(
            urls.GetSolicitado,
            {
                DocRef: $("#DocumentoReferencia").val(),
                MaterialCodigo: $("#MaterialCodigo").val(),
                BodegaId: $('#BodegaId').val(),
                DocTipo: DocTipo
            },
            function (response) {
                //console.log(response);
                $('#MaterialId').val('');
                $('#MaterialNombre').val('');
                $('#MaterialUM').val('');
                $('#MaterialPrecio').val('');
                $('#MaterialExistencia').val('');
                $('#MaterialMinimo').val('');
                $('#MaterialSolicitado').val('');
                $('#PacienteId').val('');
                $('#PacienteRM').val('');
                $('#PacienteNombreCompleto').val('');
                if (response.success && response.data != null) {
                    $('#MaterialId').val(response.data.MaterialId);
                    $('#MaterialNombre').val(response.data.MaterialNombre);
                    $('#MaterialUM').val(response.data.MaterialUM);
                    $('#MaterialPrecio').val(response.data.MaterialPrecio);
                    $('#MaterialExistencia').val(response.data.MaterialExistencia);
                    $('#MaterialMinimo').val(response.data.MaterialMinimo);
                    $('#MaterialSolicitado').val(response.data.MaterialSolicitado);
                    $('#PacienteId').val(response.data.PacienteId);
                    $('#PacienteRM').val(response.data.PacienteRM);
                    $('#PacienteNombreCompleto').val(response.data.PacienteNombreCompleto);
                    //$('#Cantidad').focus();
                }
            }
        );
    });
    $('#ProveedorNit').on('blur', function () {
        $.ajax({
            type: "GET",
            url: urls.GetProveedor,
            data: { val: $("#ProveedorNit").val() },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#ProveedorNombre').val('');
                $('#ProveedorId').val('');
                //console.log(response);
                if (response.success) {
                    $('#ProveedorNombre').val(response.data.Descripcion);
                    $('#ProveedorId').val(response.data.ProveedorId);
                    $('#PacienteRM').focus();
                }
            },
            failure: function (response) {
                console.log(response);
                jshelper.failure();
            }
        });
    });
    $('#PacienteRM').on('blur', function () {
        $.ajax({
            type: "GET",
            url: urls.GetPaciente,
            data: { registro: $(this).val() },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#PacienteId').val('');
                $('#PacienteNombreCompleto').val('');

                if (response.success) {
                    if (response.data != null) {                    
                        $('#PacienteId').val(response.data.PacienteId);
                        $('#PacienteNombreCompleto').val(response.data.Persona.Name);
                        $('#Observacion').focus();
                    }                    
                    if (response.isNew) {
                        $('#PacienteRM').removeData('previousValue');
                        $('#form-child').validate().element('#PacienteRM');
                    }
                } 
            },
            failure: function (response) {
                console.log(response);
                jshelper.failure();
            }
        });
    });
    $('#Cantidad').on('input', () => {
        let cantidad = parseFloat($('#Cantidad').val()) || 0;
        let minimo = parseFloat($('#MaterialMinimo').val()) || 0;
        let existencia = parseFloat($('#MaterialExistencia').val()) || 0;
        let newExistence = existencia - cantidad;       
        
        if (newExistence <= minimo && cantidad > 0) {
            $('#invInfo').show();
        } else {
            $('#invInfo').hide();
        }
    });

    
    // Autocomplete 
    $("#BodegaNombre").autocomplete({
        maxShowItems: 5,
        source: function (request, response) {
            $.ajax({
                url: urls.FindBodegas,
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
        }
    }).each(function (idx, ele) {
        $(ele).data("ui-autocomplete")._renderItem = function (ul, item) {
            var item = $('<div class="list_item_container"><div class="label"><strong>' + item.value + '</strong></div><div class="description" style="font-size: smaller;">' + item.desc + '</div></div>')
            return $("<li>").append(item).appendTo(ul);
        }
    });
    $("#DepartamentoCodigo").autocomplete({
        maxShowItems: 5,        
        source: function (request, response) {
            $.ajax({
                url: urls.FindDestinos,
                dataType: "json",
                data: "val=" + request.term,
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



    $("#BodegaNombre").autocomplete({
        maxShowItems: 5,
        source: function (request, response) {
            $.ajax({
                url: urls.FindBodegas,
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

    $("#MaterialCodigo").autocomplete({
        maxShowItems: 5,
        source: function (request, response) {
            $.ajax({
                url: materialSourceUrl(),
                dataType: "json",
                data: { filter: request.term, document: $('#DocumentoReferencia').val() }, //"id=" + request.term,
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
                url: urls.FindProveedores,
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

    //Todo on complete 
    $('#Cantidad').trigger('input');
});

const materialSourceUrl = () => {
    switch ($('#TipoDocumentoReferencia').val()) {
        case 'SD':
            return urls.GetMaterialSD;
            break;
        case 'SM':
            return urls.GetMaterialSM;
            break;
        default:
            return urls.FindMateriales
    }
}