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
        }

        [TestMethod]

        public void AlInstanciarUnCocinero_SeEspera_PedidosCero()
        {
            //arrange
            int resultadoEsperado = 0;

            //act
            Cocinero<Hamburguesa> cocinero = new Cocinero<Hamburguesa>("Ramon");

            //assert
            Assert.AreEqual(resultadoEsperado, cocinero.CantPedidosFinalizados);
        }
    }
}