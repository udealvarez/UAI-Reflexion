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
            throw new NotImplementedException();
        }
    }
}
