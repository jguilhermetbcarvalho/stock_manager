using AMAPA.Models;
using AMAPA.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Runtime.Intrinsics.X86;
using System.Security.AccessControl;
using System.Net.Mail;
using static AMAPA.Filter.FilterPerfil;


namespace AMAPA.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly INotyfService _toastNotification;
        string conect = "";

        public LoginController(IConfiguration configuration, INotyfService toastNotification)
        {
            _configuration = configuration;
            _toastNotification = toastNotification;
            _usuarioRepository = new UsuarioRepository(_configuration);
            conect = _configuration["ConectString"];
        }

        public ActionResult Index()
        {
            var usuarios = _usuarioRepository.BuscarUsuarios().Where(u => u.DESCRICAO_TIPO_USUARIO == "DIRETORIA").ToList();
            var executoreComNomeFormatado = usuarios.Select(u => new
            {
                ID_USUARIO = u.ID_USUARIO,
                NOME_FORMATADO = $"{u.ID_USUARIO} - {u.NOME_USUARIO}"
            });

            ViewBag.usuarios = new SelectList(executoreComNomeFormatado, "ID_USUARIO", "NOME_FORMATADO");
            ViewBag.empresa = new SelectList(_usuarioRepository.BuscarEmpresa("select * from Empresas;"), "Id", "Nome");
            ViewBag.filial = new SelectList(_usuarioRepository.BuscarFilial("select * from filiais;"), "Id", "Nome");

            return View();
        }

        [Filtro]
        public ActionResult ConfigLogin()
        {
            var usuarios = _usuarioRepository.BuscarUsuarios();
            return View(usuarios);
        }

        public ActionResult CadastrarSenhaGestao(int userId, string userName)
        {
            ViewBag.UserId = userId;
            ViewBag.UserName = userName;
            return View();
        }


        public ActionResult AlterarSenhaUsuario(int idUsuario, string senhaLiberacao, string senhaAcesso, bool naoValidarSenha)
        {
            try
            {
                int naoValidarSenhaInt = naoValidarSenha ? 1 : 0;
                senhaLiberacao = naoValidarSenha ? null : senhaLiberacao;

                if (naoValidarSenhaInt == 0)
                {
                    if (senhaAcesso == null && senhaLiberacao != null)
                    {
                        _usuarioRepository.AlterarSenhaUsuario(idUsuario, senhaLiberacao, senhaAcesso, naoValidarSenhaInt);
                        _toastNotification.Success($"Usuário {idUsuario} alterado com sucesso!");
                        _toastNotification.Information($"Alterado senha de liberação do usuario {idUsuario}.");
                    }
                    else if (senhaAcesso != null && senhaLiberacao == null)
                    {
                        _usuarioRepository.AlterarSenhaUsuario(idUsuario, senhaLiberacao, senhaAcesso, naoValidarSenhaInt);
                        _toastNotification.Success($"Usuário {idUsuario} alterado com sucesso!");
                        _toastNotification.Information($"Alterado senha de acesso {idUsuario}.");
                    }
                    else if (senhaAcesso == null && senhaLiberacao == null)
                    {
                        _toastNotification.Error($"Preencha algum dos campos. Caso deseje tirar a senha de liberação do usuário {idUsuario}, marque o checkbox.");
                    }
                    else
                    {
                        _usuarioRepository.AlterarSenhaUsuario(idUsuario, senhaLiberacao, senhaAcesso, naoValidarSenhaInt);
                        _toastNotification.Success($"Usuário {idUsuario} alterado com sucesso!");
                        _toastNotification.Information($"Alterado senha de liberação e de acesso {idUsuario}.");
                    }
                }
                else if (naoValidarSenhaInt == 1)
                {
                    _usuarioRepository.AlterarSenhaUsuario(idUsuario, senhaLiberacao, senhaAcesso, naoValidarSenhaInt);
                    _toastNotification.Success($"Usuário {idUsuario} alterado com sucesso!");
                    _toastNotification.Information($"Foi retirado a senha de liberação do usuário {idUsuario}");
                }
            }
            catch (Exception ex)
            {
                _toastNotification.Error("Erro, tente novamente.");
            }

            return RedirectToAction("ConfigLogin", "Login");
        }

        public ActionResult PrimeiroAcesso(int idUsuario, string senhaAcesso)
        {
            try
            {
                _usuarioRepository.AlterarSenhaUsuario(idUsuario, null, senhaAcesso, 0);
                _toastNotification.Success("Senha cadastrada.");
            }
            catch (Exception ex)
            {
                _toastNotification.Error("Erro, tente novamente.");
            }

            return RedirectToAction("ConfigLogin", "Login");
        }
        #region Login
        [HttpPost]
        public async Task<IActionResult> Index(string login, string senha, int empresa, int filial)
        {
            var user1 = await _usuarioRepository.BuscarUsuarioAsync(login, senha);

            // Verificar se a senha de gestão é nula
            if (user1 != null && string.IsNullOrEmpty(user1.m_Item1.SENHA_ACESSO_GESTOR))
            {
                return RedirectToAction("CadastrarSenhaGestao", "Login", new { userId = user1.m_Item1.ID_USUARIO, userName = user1.m_Item1.NOME_USUARIO });
            }

            if (user1 != null && user1.m_Item2 == true)
            {
                Usuarios user = new Usuarios
                {
                    NOME_USUARIO = user1.m_Item1.NOME_USUARIO,
                    CPF = user1.m_Item1.CPF,
                    ID_EMPRESA = empresa,
                    ID_FILIAL = filial,
                    ID_USUARIO = Convert.ToInt32(user1.m_Item1.ID_USUARIO)
                };

                string usuario = JsonConvert.SerializeObject(user);
                HttpContext.Session.SetString("user", usuario);
                _toastNotification.Success("Bom Trabalho Sr(a) " + user.NOME_USUARIO);
                return RedirectToAction("ConfigLogin", "Login");
            }
            ViewBag.empresa = new SelectList(_usuarioRepository.BuscarEmpresa("select * from Empresas;"), "Id", "Nome");
            ViewBag.filial = new SelectList(_usuarioRepository.BuscarFilial("select * from filiais;"), "Id", "Nome");
            ViewBag.usuarios = new SelectList(_usuarioRepository.BuscarUsuarios().Where(u => u.DESCRICAO_TIPO_USUARIO == "DIRETORIA").ToList(), "ID_USUARIO", "NOME_USUARIO");
            _toastNotification.Error("Tente novamente ");
            return View();
        }


        public void Sair()
        {
            HttpContext.Session.Clear();
        }
        #endregion

    }
}
