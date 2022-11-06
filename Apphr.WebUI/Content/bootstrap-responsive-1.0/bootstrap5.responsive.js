(function ( $ ) {    
    var columnTitles = [];
    var columnHiddensIx = [];
    var columnsCount;

    // Plugin definition.
    $.fn.STRB5 = function(options) {
        var settings = $.extend($.fn.STRB5.defaults, options );               
       
        return this.each(function() 
        {
            // Do something to each element here.
            elem = $(this);
            //elem.css( "color", "green" );
            setVars();
            setDom();
            bindEvents();
            setSize();
        });
    };

    // definimos los parámetros junto con los valores por defecto de la función
    $.fn.STRB5.defaults = {
        // para el fondo un color por defecto
        background: '#a6cdec'
    };

    function setVars()
    {    
        elemParent = elem.parent();
        // Recupera los titulos de las columnas
        elem.find('tr:first>th').each(function (index, tr) {
            columnTitles.push($(tr).html());		
        });
        columnsCount = columnTitles.length;
        //console.log(columnTitles);
        //console.log(columnsCount);
    };

    function setDom()
    {
        // Numerar las filas y columnas
        elem.find('tbody>tr').each(function(index,tr){
            $(this).attr("data-rowid", index);
            $(this).find('td,th').attr("data-colid", function(index){return index})
        });
        // agrega columna al fina de cada linea exceputando la que tiene clase "no-register"
        elem.find("tr:not('.no-register')").each(function()
        {
            var fila = $(this).attr("data-rowid");          
            $(this).append('<td class="row-control"><p><button class="btn btn-Secundary text-success" type="button" data-bs-toggle="collapse" data-bs-target="#collapse' + fila + '" aria-expanded="false" aria-controls="collapseWidthExample"><i class="fas fa-info"></i></button></p></td>');        
        });

        elem.find(".no-register>td").attr('colspan', (columnsCount + 1));
        //alert(columnsCount);

    };

    function bindEvents() {
        $(window).on('resize', setSize.bind());
    };

    function setSize()
    {
        // mostrar todas la columnas
        elem.find('tr:first>th').each(function (index, tr) {
            $('td:nth-child('+(index+1)+'),th:nth-child('+(index+1)+')').show();		
        });

        // inicializar la variables con el ancho de collapse
        var widthAcum = elem.find('tr:first>td.row-control').outerWidth(true);
        //alert(widthAcum);

        // ocultar columnas
        for (var i = 1; i <= columnsCount; i+=1) 
        {
            var columnToHide = elem.find('tr:first>th:nth-child(' + i + ')');
            console.log(columnToHide.html());
            if (!columnToHide.hasClass('row-control')){
                widthAcum = widthAcum + columnToHide.outerWidth(true);
                if(widthAcum > elemParent.width())
                { 
                    $('td:nth-child('+ i +'),th:nth-child('+ i +')').hide();
                }
            }         
            else{
                alert('tiene Clase');
            }   
        }
        // almacenar id de columnas ocultas
        columnHiddensIx = [];
        elem.find('tr:first>th:hidden').each(function (index, tr) {
            columnHiddensIx.push($(this).attr('data-colid'));            
        });

        //llenar detalles
        elem.find('tbody>tr[data-rowid]:not(:first)').each(function (){
            fila = $(this).attr("data-rowid"); 
            var noexiste = $('tr[data-forrow-id="' + fila + '"]').length == 0;
            console.log(noexiste);
           
                contenido = '<ul class="row-details" data-row-index="'+fila+'">';
                var hasDetail = 0;
                for (var i = 0; i < columnHiddensIx.length; i+=1) 
                {
                    columna = columnHiddensIx[i];
                    contenido = contenido + '<li data-row="' + fila + '" data-column="'+ columna +'">' + 
                    '<span class="row-title">'+columnTitles[columnHiddensIx[i]]+':&nbsp</span>' + 
                    '<span class="row-value">' + getCellValue(fila, columna) + '</span>'+
                    '</li>';        
                    //console.log("En el índice '" + i + "' hay este valor: " + columnsTitle[columnsHidden[i]]);
                    //i++;
                }
                contenido = contenido + '</ul>';
                //console.log(contenido);
            
            if(noexiste)
            {
                $(this).after('<tr class="collapse collapse-horizontal" id="collapse'+fila+'" data-forrow-id="'+fila+'"><td colspan="'+ columnsCount +'">'+ contenido +'</td></tr>');
            } 
            else
            {
                $('tr[data-forrow-id="' + fila + '"]').html('<td colspan="' + columnsCount + '">'+ contenido +'</td>');
            }
        });
        //console.log(columnHiddensIx);
    };
    function getCellValue(row, col)
    {
        return elem.find('tr[data-rowid="' + row + '"]>td[data-colid="' + col + '"]').html();
    };

    // Private function for debugging.
    function debug( obj ) 
    {
        if ( window.console && window.console.log ) {
            window.console.log( "hilight selection count: " + obj.length );
        }
    };
 
}( jQuery ));