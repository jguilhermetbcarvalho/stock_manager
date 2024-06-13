using AMAPA.Models;
using AMAPA.Repository;
using AMAPA.Repositoryusing;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using System.Security;
using static AMAPA.Filter.FilterPerfil;

namespace AMAPA.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly MovimentoRepository _movimentoRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly ProdutoRepository _produtoRepository;
        private readonly DashboardRepository _dashRepository;
        private readonly INotyfService _toastNotification;
        string conect = "";

        public ProdutoController(IConfiguration configuration, INotyfService toastNotification)
        {
            _configuration = configuration;
            _movimentoRepository = new MovimentoRepository(_configuration);
            _usuarioRepository = new UsuarioRepository(_configuration);
            _produtoRepository = new ProdutoRepository(_configuration);
            _dashRepository = new DashboardRepository(_configuration);
            _usuarioRepository = new UsuarioRepository(_configuration);
            _toastNotification = toastNotification;
            conect = _configuration["ConectString"];
        }

        #region Dashboard
        public IActionResult VisaoGeral()
        {
            ViewBag.CurrentPage = "VisaoGeral";
            var quantidadeLancado = _dashRepository.ContarRegistrosCarrinhoLancado();
            var quantidadeDevolvido = _dashRepository.ContarRegistrosCarrinhoDevolvido();
            var quantidadePendente = _dashRepository.ContarRegistrosPendente();
            var quantidadeEntregue = _dashRepository.ContarRegistrosEntregue();
            ViewBag.carrinho = quantidadeLancado;
            ViewBag.devolvido = quantidadeDevolvido;
            ViewBag.pendente = quantidadePendente;
            ViewBag.entregue = quantidadeEntregue;
            return View();
        }

        public IActionResult VerifCodBarras()
        {
            ViewBag.CurrentPage = "VerifCodBarras";
            var produtos = _produtoRepository.BuscarProdutosCodBarrasDivergente();
            return View(produtos);
        }

        public ActionResult ConfirmarAlteracaoProduto(string idProduto)
        {
            try
            {
                _produtoRepository.ConfirmarAlteracaoProduto(idProduto);
                _toastNotification.Success("Retirado da lista de verificação!");

            }
            catch (Exception ex)
            {
                _toastNotification.Error("Erro ao tirar da lista de verificação");
            }

            return RedirectToAction("VerifCodbarras", "Produto");
        }

        public IActionResult TeclasAtalho()
        {
            ViewBag.CurrentPage = "TeclasAtalho";
            return View();
        }
        #endregion

        #region Produtos Devolvidos
        public IActionResult ProdutoDevolvido()
        {
            ViewBag.CurrentPage = "ProdutoDevolvido";
            var movimentos = _movimentoRepository.BuscarMovimentoDevolvido();
            return View(movimentos);
        }
        #endregion

        #region Produtos Entregues
        public IActionResult ProdutoEntregue()
        {
            ViewBag.CurrentPage = "ProdutoEntregue";
            var usuarios = _usuarioRepository.BuscarUsuarios();
            var executoreComNomeFormatado = usuarios.Select(u => new
            {
                ID_USUARIO = u.ID_USUARIO,
                NOME_FORMATADO = $"{u.ID_USUARIO} - {u.NOME_USUARIO}"
            });

            ViewBag.EXECUTORES = new SelectList(executoreComNomeFormatado, "ID_USUARIO", "NOME_FORMATADO");
            var movimentos = _movimentoRepository.BuscarMovimentoEntregue();

            return View(movimentos);
        }

        [HttpPost]
        public ActionResult RemoverProdutoEntregue(string produtosRemover, int idExecutor, string login, string senha = "")
        {
            try
            {
                var user = _usuarioRepository.BuscarUsuarioLiberacao(login, senha);
                var idEstoquista = int.Parse(login);
                List<EntregarProdutos> listaProdutos = new List<EntregarProdutos>();
                listaProdutos = JsonConvert.DeserializeObject<List<EntregarProdutos>>(produtosRemover);

                if (user)
                {
                    foreach (var produto in listaProdutos)
                    {
                        int idMovimento = produto.idMovimento;
                        string idProduto = produto.idProduto;
                        string identificaOs = produto.identificaOs;
                        int quantInteira = produto.qtdeInteira;
                        string nomeProduto = produto.nomeProduto;
                        string fabricProduto = produto.fabricProduto;


                        var resultadoSituacao = _movimentoRepository.BuscarSituacaoMovimento(idMovimento);
                        var resultadoRemover = _movimentoRepository.RemoverProdutoEntregue(idMovimento, idProduto, quantInteira, idEstoquista, nomeProduto, fabricProduto);

                        // Aqui pode mudar para resultadoRemover == 1 && (resultadoSituacao == 1 || resultadoSituacao == 4)
                        if (resultadoRemover == 1 && (resultadoSituacao == 1 || resultadoSituacao == 4))
                        {
                            _toastNotification.Success($"Produto {nomeProduto} removido com sucesso!");
                        }
                        else if (resultadoRemover == 2)
                        {
                            _toastNotification.Error("Contém apenas um produto no movimento. Para remover o produto, adicione outro produto ou exclua o movimento pelo Solution.");
                        }
                        else
                        {
                            _toastNotification.Error($"Não foi possível remover o produto {nomeProduto} do movimento.");
                        }
                    }
                }
                else
                {
                    _toastNotification.Error("Senha incorreta.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ProdutoEntregue", "Produto");
        }

        #endregion

        #region Produtos Pendentes
        public IActionResult ProdutoPendente()
        {
            ViewBag.CurrentPage = "ProdutoPendente";
            var usuarios = _usuarioRepository.BuscarUsuarios();
            var executoreComNomeFormatado = usuarios.Select(u => new
            {
                ID_USUARIO = u.ID_USUARIO,
                NOME_FORMATADO = $"{u.ID_USUARIO} - {u.NOME_USUARIO}"
            });

            ViewBag.EXECUTORES = new SelectList(executoreComNomeFormatado, "ID_USUARIO", "NOME_FORMATADO");
            var movimentos = _movimentoRepository.BuscarMovimentoProdutoPendente();
            return View(movimentos);
        }

        [HttpPost]
        public ActionResult ConfirmarEntregaProdutoPendente(string produtosEntrega, int idExecutor, string login, string senha = "")
        {
            try
            {
                var user = _usuarioRepository.BuscarUsuarioLiberacao(login, senha);
                var idEstoquista = int.Parse(login);
                List<EntregarProdutos> listaProdutos = new List<EntregarProdutos>();
                listaProdutos = JsonConvert.DeserializeObject<List<EntregarProdutos>>(produtosEntrega);

                if (user)
                {
                    foreach (var produto in listaProdutos)
                    {
                        int idMovimento = produto.idMovimento;
                        string idProduto = produto.idProduto;
                        string identificaOs = produto.identificaOs;
                        int qtdeInteira = produto.qtdeInteira;
                        string nomeProduo = produto.nomeProduto;

                        _movimentoRepository.ConfirmarEntregaProdutoPendente(idMovimento, idProduto, idExecutor, idEstoquista);
                        _toastNotification.Success($"Produto {nomeProduo} entregue com sucesso!");
                    }

                }
                else
                {
                    _toastNotification.Error("Senha incorreta.");
                }
            }
            catch (Exception ex)
            {
                _toastNotification.Error("Ocorreu um erro ao confirmar o produto.");
            }

            return RedirectToAction("ProdutoPendente", "Produto");
        }

        [HttpPost]
        public ActionResult VerificarCodBarras(string idProduto, string codBarrasGestor, string locProduto)
        {
            try
            {
                bool codigosIguais = false;

                var listaCodBarras = _produtoRepository.BuscarCodigoBarrasProduto(idProduto);

                foreach (var codBarrasSolution in listaCodBarras)
                {
                    if (codBarrasSolution == codBarrasGestor)
                    {
                        codigosIguais = true;
                        
                        break;
                    }
                    
                }

                if (!codigosIguais)
                {
                    _produtoRepository.VerificarCodBarras(idProduto, locProduto, codBarrasGestor);
                }

                return Json(codigosIguais);
            }
            catch (Exception ex)
            {
                _toastNotification.Error("Erro ao verificar o código de barras");
                return StatusCode(500, "Erro interno do servidor");
            }
        }
        #endregion

        #region Produtos Carrinho
        public IActionResult ProdutoCarrinho()
        {
            ViewBag.CurrentPage = "ProdutoCarrinho";
            var usuarios = _usuarioRepository.BuscarUsuarios();
            var executoreComNomeFormatado = usuarios.Select(u => new
            {
                ID_USUARIO = u.ID_USUARIO,
                NOME_FORMATADO = $"{u.ID_USUARIO} - {u.NOME_USUARIO}"
            });

            ViewBag.EXECUTORES = new SelectList(executoreComNomeFormatado, "ID_USUARIO", "NOME_FORMATADO");
            var movimentos = _movimentoRepository.BuscarMovimentoProdutoCarrinho();
            return View(movimentos);
        }

        public ActionResult AdicionarProdutoCarrinho(string idProduto, string descricao, string codigoBarras, string codigoFabricante, int quantInteira, string veiculo, int idExecutor, int idEstoquista, string localizacao)
        {
            try
            {
                var resultado = _movimentoRepository.AdicionarProdutoCarrinho(idProduto, descricao, codigoBarras, codigoFabricante, quantInteira, veiculo, idExecutor, idEstoquista, localizacao);
                if (resultado == 1)
                {
                    _toastNotification.Success($"Produto {descricao} adicionado no carrinho com sucesso!");
                }
                else
                {
                    _toastNotification.Error("Saldo insuficiente no estoque.");
                }


            }
            catch (Exception ex)
            {
                _toastNotification.Error($"Ocorreu um erro ao adicionar o produto {descricao} no carrinho.");
            }

            return RedirectToAction("ProdutoCarrinho", "Produto");
        }

        public ActionResult EditarQuantidadeProdutoCarrinho(int idPk, string idProduto, string nomeProduto, int novaQuantInteira, int quantInteira)
        {
            try
            {
                var resultEdit = _movimentoRepository.EditarQuantidadeProdutoCarrinho(idPk, idProduto, novaQuantInteira, quantInteira);

                if (resultEdit == 1)
                {
                    _toastNotification.Success($"Produto {nomeProduto} alterado no carrinho com sucesso!");
                }
                else
                {
                    _toastNotification.Error($"Saldo insuficiente do produto {nomeProduto} no estoque.");
                }
            }
            catch (Exception ex)
            {
                _toastNotification.Error($"Ocorreu um erro ao adicionar o produto {nomeProduto} no carrinho.");
            }

            return RedirectToAction("ProdutoCarrinho", "Produto");
        }

        [HttpPost]
        public ActionResult RemoverProdutoCarrinho(string produtosDevolver, string login, string senha = "")
        {
            try
            {
                var user = _usuarioRepository.BuscarUsuarioLiberacao(login, senha);
                var idEstoquista = int.Parse(login);
                List<EntregarProdutos> listaProdutos = new List<EntregarProdutos>();
                listaProdutos = JsonConvert.DeserializeObject<List<EntregarProdutos>>(produtosDevolver);

                if (user)
                {
                    foreach (var produto in listaProdutos)
                    {
                        int idMovimento = produto.idMovimento;
                        string idProduto = produto.idProduto;
                        string identificaOs = produto.identificaOs;
                        int qtdeInteira = produto.qtdeInteira;
                        string nomeProduo = produto.nomeProduto;

                        _movimentoRepository.RemoverProdutoCarrinho(idProduto, identificaOs, idEstoquista, qtdeInteira);
                        _toastNotification.Success($"Produto {nomeProduo} devolvido com sucesso!");
                    }
                }
                else
                {
                    _toastNotification.Error("Senha incorreta.");
                }

            }
            catch (Exception ex)
            {
                _toastNotification.Error("Ocorreu um erro ao devolver o produto.");
            }

            return RedirectToAction("ProdutoCarrinho", "Produto");
        }

        [HttpPost]
        public ActionResult VincularProdutoCarrinho(string produtosVincular, int idMovimento, string login, string senha = "")
        {
            var user = _usuarioRepository.BuscarUsuarioLiberacao(login, senha);
            var situacao = _movimentoRepository.BuscarSituacaoMovimento(idMovimento);
            var idVinculacao = int.Parse(login);
            List<EntregarProdutos> listaProdutos = new List<EntregarProdutos>();
            listaProdutos = JsonConvert.DeserializeObject<List<EntregarProdutos>>(produtosVincular);

            if (user)
            {
                if (situacao == 1 || situacao == 4)
                {
                    foreach (var produto in listaProdutos)
                    {
                        string idProduto = produto.idProduto;
                        string identificaOs = produto.identificaOs;
                        int qtdeInteira = produto.qtdeInteira;
                        string nomeProduto = produto.nomeProduto;
                        string fabricProduto = produto.fabricProduto;
                        int idExecutor = produto.idExecutor;
                        int idEstoquista = produto.idEstoquista;
                        var prod = _produtoRepository.BuscarProdutoCodigoProdutoIndividual(idProduto);

                        int resultadoVincular = _movimentoRepository.VincularProdutoCarrinho(prod, idMovimento, identificaOs, idProduto, qtdeInteira, idExecutor, idVinculacao, idEstoquista, nomeProduto, fabricProduto);

                        if (resultadoVincular == 1)
                        {
                            _toastNotification.Success($"Produto {nomeProduto} inserido no movimento com sucesso!");
                        }
                        else if (resultadoVincular == 2)
                        {
                            _toastNotification.Success($"Produto {nomeProduto} já contém no movimento.");
                        }
                        else if (resultadoVincular == 3)
                        {
                            _toastNotification.Error("Quantidade cadastrada não coincide com a quantidade no movimento.");
                        }
                        else
                        {
                            _toastNotification.Error($"Erro ao vincular o produto {nomeProduto} ao movimento.");
                        }
                    }
                }
                else
                {
                    _toastNotification.Error("Situação do movimento inválida.");
                }
            }
            else
            {
                _toastNotification.Error("Senha incorreta.");
            }

            return RedirectToAction("ProdutoCarrinho", "Produto");
        }
        #endregion

        #region Busca de Produtos Lista
        [HttpPost]
        public IActionResult BuscarProdutoIdProdutoLista(string idProduto)
        {
            var produto = _produtoRepository.BuscarProdutoCodigoProdutoLista(idProduto);
            if (produto != null)
            {
                return Json(produto);
            }
            else
            {
                return Json(new { mensagem = false });
            }
        }

        [HttpPost]
        public IActionResult BuscarProdutoCodigoBarrasLista(string codigoBarras)
        {
            var produto = _produtoRepository.BuscarProdutoCodigoBarrasLista(codigoBarras);

            if (produto != null)
            {
                return Json(produto);
            }
            else
            {
                return Json(new { mensagem = false });
            }
        }

        [HttpPost]
        public IActionResult BuscarProdutoCodigoFabricanteLista(string codigoFabricante)
        {
            var produto = _produtoRepository.BuscarProdutoCodigoFabricanteLista(codigoFabricante);
            if (produto != null)
            {
                return Json(produto);
            }
            else
            {
                return Json(new { mensagem = false });
            }
        }
        #endregion

        #region Busca de Produtos Individualmente
        [HttpPost]
        public IActionResult BuscarProdutoIdProdutoIndividual(string idProduto)
        {
            var produto = _produtoRepository.BuscarProdutoCodigoProdutoIndividual(idProduto);
            if (produto != null && !string.IsNullOrEmpty(produto.ID_PRODUTO))
            {
                return Json(produto);
            }
            else
            {
                return Json(new { mensagem = false });
            }
        }

        [HttpPost]
        public IActionResult BuscarProdutoCodigoBarrasIndividual(string codigoBarras)
        {
            var produto = _produtoRepository.BuscarProdutoCodigoBarrasIndividual(codigoBarras);

            if (produto != null && !string.IsNullOrEmpty(produto.CODIGO_BARRAS))
            {
                return Json(produto);
            }
            else
            {
                return Json(new { mensagem = false });
            }
        }

        [HttpPost]
        public IActionResult BuscarProdutoCodigoFabricanteIndividual(string codigoFabricante)
        {
            var produto = _produtoRepository.BuscarProdutoCodigoFabricanteIndividual(codigoFabricante);
            if (produto != null && !string.IsNullOrEmpty(produto.CODIGO_FABRICANTE))
            {
                return Json(produto);
            }
            else
            {
                return Json(new { mensagem = false });
            }
        }
        #endregion

    }
}