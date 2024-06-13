// Busca de Produtos Individualmente
function BuscarProdutoIdProdutoIndividual() {
    var idProd = $("#idProd").val();
    if (idProd != '') {
        $.ajax({
            url: "/Produto/BuscarProdutoIdProdutoIndividual",
            type: 'POST',
            dataType: 'json',
            data: { idProduto: idProd },
            success: function (data) {
                if (data.mensagem === false) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Produto n&atildeo encontrado',
                        showConfirmButton: false,
                        timer: 1200    
                    });
                } else {
                    console.log(data)
                    $("#codBarras").val(data.codigO_BARRAS);
                    $("#codFrab").val(data.codigO_FABRICANTE);
                    $("#descricao").val(data.descricao);
                    $("#loc").val(data.localizacao);
                    $('#quantInteira').attr('placeholder', 'Quantidade disponivel: ' + data.estoquE_ATUAL);
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
}

function BuscarProdutoCodigoBarrasIndividual() {
    var codBarras = $("#codBarras").val();
    if (codBarras != '') {
        $.ajax({
            url: "/Produto/BuscarProdutoCodigoBarrasIndividual",
            type: 'POST',
            dataType: 'json',
            data: { codigoBarras: codBarras },
            success: function (data) {
                if (data.mensagem === false) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Produto n&atildeo encontrado',
                        showConfirmButton: false,
                        timer: 1200    
                    });
                } else {
                    $("#idProd").val(data.iD_PRODUTO);
                    $("#codFrab").val(data.codigO_FABRICANTE);
                    $("#descricao").val(data.descricao);
                    $("#loc").val(data.localizacao);
                    $('#quantInteira').attr('placeholder', 'Quantidade disponivel: ' + data.estoquE_ATUAL);
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
}

function BuscarProdutoCodigoFabricanteIndividual() {
    var codFrab = $("#codFrab").val();
    if (codFrab != '') {
        $.ajax({
            url: "/Produto/BuscarProdutoCodigoFabricanteIndividual",
            type: 'POST',
            dataType: 'json',
            data: { codigoFabricante: codFrab },
            success: function (data) {
                if (data.mensagem === false) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Produto n&atildeo encontrado',
                        showConfirmButton: false,
                        timer: 1200    
                    });
                } else {
                    console.log(data)
                    $("#idProd").val(data.iD_PRODUTO);
                    $("#codBarras").val(data.codigO_BARRAS);
                    $("#descricao").val(data.descricao);
                    $("#loc").val(data.localizacao);
                    $('#quantInteira').attr('placeholder', 'Quantidade disponivel: ' + data.estoquE_ATUAL);
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
}


// Busca de Produtos Lista
function BuscarProdutoIdProdutoLista() {
    var input = document.getElementById("idProd");
    var searchText = input.value.trim();
    if (searchText.length >= 3) {
        $.ajax({
            url: '/Produto/BuscarProdutoIdProdutoLista',
            method: 'POST',
            data: { idProduto: searchText },
            success: function (data) {
                var dropdown = document.getElementById("search-idProd");
                dropdown.innerHTML = "";
                data.forEach(function (item) {
                    var listItem = document.createElement("li");
                    listItem.textContent = item.iD_PRODUTO;
                    listItem.value = item.iD_PRODUTO;
                    listItem.addEventListener("click", function () {
                        input.value = item.iD_PRODUTO;
                        dropdown.style.display = "none";
                    });
                    dropdown.appendChild(listItem);
                });
                dropdown.style.display = "block";

                document.addEventListener("click", function (event) {
                    if (!input.contains(event.target) && !dropdown.contains(event.target)) {
                        dropdown.style.display = "none";
                    }
                });
            }
        });
    } else {
        document.getElementById("search-idProd").style.display = "none";
    }
}

function BuscarProdutoCodigoBarrasLista() {
    var input = document.getElementById("codBarras");
    var searchText = input.value.trim();
    if (searchText.length >= 3) {
        $.ajax({
            url: '/Produto/BuscarProdutoCodigoBarrasLista',
            method: 'POST',
            data: { codigoBarras: searchText },
            success: function (data) {
                var dropdown = document.getElementById("search-codBarras");
                dropdown.innerHTML = "";
                data.forEach(function (item) {
                    var listItem = document.createElement("li");
                    listItem.textContent = item.codigO_BARRAS;
                    listItem.value = item.codigO_BARRAS;
                    listItem.addEventListener("click", function () {
                        input.value = item.codigO_BARRAS;
                        dropdown.style.display = "none";
                    });
                    dropdown.appendChild(listItem);
                });
                dropdown.style.display = "block";

                document.addEventListener("click", function (event) {
                    if (!input.contains(event.target) && !dropdown.contains(event.target)) {
                        dropdown.style.display = "none";
                    }
                });
            }
        });
    } else {
        document.getElementById("search-codBarras").style.display = "none";
    }
}

function BuscarProdutoCodigoFabricanteLista() {
    var input = document.getElementById("codFrab");
    var searchText = input.value.trim(); // Remover espaços em branco antes e depois
    if (searchText.length >= 3) {
        // Enviar solicitação AJAX para buscar produtos por código de fabricante
        $.ajax({
            url: '/Produto/BuscarProdutoCodigoFabricanteLista', // URL para sua ação que busca produtos por código de fabricante
            method: 'POST',
            data: { codigoFabricante: searchText },
            success: function (data) {
                var dropdown = document.getElementById("search-codFrab");
                dropdown.innerHTML = ""; // Limpar o dropdown atual
                // Adicionar os resultados ao dropdown
                data.forEach(function (item) {
                    console.log(item)
                    var listItem = document.createElement("li");
                    listItem.textContent = item.codigO_FABRICANTE;
                    listItem.value = item.codigO_FABRICANTE;
                    listItem.addEventListener("click", function () {
                        input.value = item.codigO_FABRICANTE; // Definir o valor do campo de entrada com o valor selecionado
                        dropdown.style.display = "none"; // Ocultar o dropdown após selecionar
                    });
                    dropdown.appendChild(listItem);
                });
                // Mostrar o dropdown
                dropdown.style.display = "block";

                // Adicionar um listener de clique ao documento para fechar o dropdown quando clicar fora do campo de busca
                document.addEventListener("click", function (event) {
                    if (!input.contains(event.target) && !dropdown.contains(event.target)) {
                        dropdown.style.display = "none";
                    }
                });
            }
        });
    } else {
        // Se o texto for menor que 3 caracteres, esconder o dropdown
        document.getElementById("search-codFrab").style.display = "none";
    }
}

// Limpar os campos de entrada e de pesquisa
$(document).ready(function () {
    $('.btn-fechar-modal').click(function () {
        
        $('input[type="text"]').val('');
        $('input[type="search"]').val('');
    });

    $('.btn-verificar').click(function () {

        $('input[type="text"]').val('');
        $('input[type="search"]').val('');
    });
});

function validarQuantidade() {
    var quantidade = document.getElementById("quantInteira").value;

    if (quantidade <= 0) {
        Swal.fire({
            icon: 'error',
            title: 'Por favor, insira uma quantidade maior que zero.'
        });
        return false;
    }
    return true;
}



