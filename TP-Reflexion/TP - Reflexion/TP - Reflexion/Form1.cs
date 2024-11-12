using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TP___Reflexion
{
    public partial class Form1 : Form
    {
        private Dictionary<string, Type> clases = new Dictionary<string, Type>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())/*uso el control para conseguir el path de una*/
            {
                openFileDialog.Filter = "Archivos de C# (*.cs)|*.cs|Todos los archivos (*.*)|*.*";
                openFileDialog.Title = "Seleccionar archivo .cs";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    CargarClases(filePath);
                }
            }
        }

        private void CargarClases(string filePath)
        {
            string code = File.ReadAllText(filePath);//leer todo el contenido del archivo que le paso por filepath
            var tree = CSharpSyntaxTree.ParseText(code);//se genera un "arbol de sintaxis" de la variable que genere antes
            var root = tree.GetRoot();//me paro en el nodo raiz que tiene la totalidad del codigo
            var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();//DescendantNodes() recupera todos los nodos que están en el árbol de sintaxis,
                                                                                            //y OfType<ClassDeclarationSyntax>() filtra esos nodos para obtener solo aquellos que representan declaraciones de clases.

            clases.Clear();//limpio el diccionariop ara no pisar otras clases viejas
            cmbClases.Items.Clear();//limpio el combo

            foreach (var classDecl in classDeclarations)//sobre cada declaracion de clase
            {
                var className = classDecl.Identifier.Text;//agarro el nombre
                var properties = classDecl.Members.OfType<PropertyDeclarationSyntax>();//agarro miembros de clase para sacar las properties

                Type tipo = CrearTipoDinamico(className, properties);//llamo al metodo
                clases[className] = tipo; // clases es el diccionario, las llaves [] es la key
                cmbClases.Items.Add(className);
            }

            if (clases.Count > 0)
            {
                cmbClases.SelectedIndex = 0; // Seleccionar la primera clase por defecto
            }
        }

        private Type CrearTipoDinamico(string className, IEnumerable<PropertyDeclarationSyntax> properties)
        {
            // Utilizando reflection emit
            var assemblyName = new AssemblyName("DynamicAssembly");//representa el ensamblado donde armo el tipo dinamico
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);//AssemblyBuilder.DefineDynamicAssembly se utiliza para crear un ensamblado dinámico que puede ser ejecutado en tiempo de ejecución.
                                                                                                                 //La opción AssemblyBuilderAccess.Run indica que el ensamblado es accesible solo para la ejecució
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");//Dentro del ensamblado, se define un módulo dinámico llamado "DynamicModule".
            var typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public);//se define un nuevo tipo dinámico con el nombre className y se especifica que es público. Esto se realiza a través del TypeAttributes que es un enum interno de la biblioteca

            foreach (var prop in properties)//Se itera sobre las propiedades que se han extraído del archivo de código.
            {
                var propertyName = prop.Identifier.Text;//obtengo nombre propiedad
                var propertyType = MapearTipo(prop.Type.ToString());// obtengo tipo de propiedad del dato mediante MapearTipo
                var fieldBuilder = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);//Se define el campo en el tipo dinámico, especificando su nombre, atributos y tipo.

                var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);//Se define la propiedad en el tipo dinámico, especificando su nombre, atributos y tipo.

                var getMethodBuilder = typeBuilder.DefineMethod("get_" + propertyName,
                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                    propertyType, Type.EmptyTypes);//Se define un método que actuará como el getter de la propiedad. Se especifican los atributos y el tipo de retorno.
                var getIL = getMethodBuilder.GetILGenerator();//Se obtiene el generador de IL (Intermediate Language) para el método. Por ejemplos MSIL

                getIL.Emit(OpCodes.Ldarg_0);
                getIL.Emit(OpCodes.Ldfld, fieldBuilder);
                getIL.Emit(OpCodes.Ret);
                propertyBuilder.SetGetMethod(getMethodBuilder);
                /*
                 * se emiten instrucciones IL que cargan el objeto actual (Ldarg_0), 
                 * acceden al campo (Ldfld) 
                 * y retornan su valor (Ret). 
                 * se asocia este método como el getter de la propiedad
                 */


                var setMethodBuilder = typeBuilder.DefineMethod("set_" + propertyName,
                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                    null, new Type[] { propertyType });//Se define un método que actuará como el setter de la propiedad. A diferencia del getter, este método no devuelve un valor.

                var setIL = setMethodBuilder.GetILGenerator();
                setIL.Emit(OpCodes.Ldarg_0);
                setIL.Emit(OpCodes.Ldarg_1);
                setIL.Emit(OpCodes.Stfld, fieldBuilder);
                setIL.Emit(OpCodes.Ret);
                propertyBuilder.SetSetMethod(setMethodBuilder);
                /*
                 * Se obtiene el generador de IL para el método setter. 
                 * Se emiten instrucciones para cargar el objeto actual,
                 * cargar el valor de entrada (Ldarg_1), 
                 * almacenar el valor en el campo (Stfld), y retornar.
                 */
            }

            return typeBuilder.CreateType();
        }

        private Type MapearTipo(string tipo)
        {
            // Mapeo de tipos de C# a tipos .NET
            return tipo switch
            {
                "int" => typeof(int),
                "string" => typeof(string),
                "DateTime" => typeof(DateTime),
                _ => typeof(string) // Tipo por defecto, se pueden seguir mapeando a mano
            };
        }

        private void btnGenerarSQL_Click(object sender, EventArgs e)
        {
            if (cmbClases.SelectedItem != null)
            {
                string nombreClase = cmbClases.SelectedItem.ToString();
                if (clases.TryGetValue(nombreClase, out Type tipoSeleccionado))
                {
                    string sql = GenerarSQL(tipoSeleccionado);
                    txtSQL.Text = sql;
                }
                else
                {
                    MessageBox.Show("Clase no encontrada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
