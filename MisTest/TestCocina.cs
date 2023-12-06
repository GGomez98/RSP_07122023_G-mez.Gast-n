using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Modelos;

namespace MisTest
{
    [TestClass]
    public class TestCocina
    {
        [TestMethod]
        [ExpectedException(typeof(FileManagerException))]
        public void AlGuardarUnArchivo_ConNombreInvalido_TengoUnaExcepcion()
        {
            //arrange

            //act

            //assert
            FileManager.Guardar("Algo", "/", true);
        }

        [TestMethod]

        public void AlInstanciarUnCocinero_SeEspera_PedidosCero()
        {
            //arrange
            int resultadoEsperado = 0;
            Cocinero<Hamburguesa> cocinero = new Cocinero<Hamburguesa>("Ramon");

            //act

            //assert
            Assert.AreEqual(resultadoEsperado, cocinero.CantPedidosFinalizados);
        }
    }
}