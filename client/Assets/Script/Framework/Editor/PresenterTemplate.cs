using System.Collections.Generic;
using System.IO;

namespace Framework
{
    class PresenterTemplate
    {
        public static string ClassName(DataContext ctx)
        {
            return ctx.Name + "Presenter";
        }

        // Presenter中的事件名
        public static string Event(DataContext ctx)
        {
            return "On" + ctx.Name + "Changed";
        }

        #region Property
        public static string Property( DataContext ctx )
        {
            return ctx.Name;
        }

        public static string PropertyType( DataContext ctx )
        {
            switch( ctx.Type )
            {
                case WidgetType.InputField:
                case WidgetType.Text:
                    return "string";
            }

            return "unknown";
        }



        static bool HasPropertyChangedNotify( DataContext ctx )
        {            
            return ctx.SyncType ==  DataSyncType.PresenterToView || ctx.SyncType == DataSyncType.TwoWay;          
        }

        static void PropertyBody(CodeGenerator gen, DataContext rootContext, DataContext ctx)
        {
            switch (ctx.Type)
            {
                case WidgetType.ScrollRect:
                    {
                        gen.PrintLine("public Framework.ObservableCollection<int, ", ClassName(ctx), "> ", Property(ctx), "Collection { get; set; }");
                    }
                    break;
                case WidgetType.Text:
                case WidgetType.InputField:
                    {
                        if (HasPropertyChangedNotify(ctx))
                        {
                            gen.PrintLine("public Action ", Event(ctx), ";");
                        }

                        gen.PrintLine("public ", PropertyType(ctx), " ", Property(ctx));
                        gen.PrintLine("{");
                        gen.In();

                        gen.PrintLine("get");
                        gen.PrintLine("{");
                        gen.In();
                        gen.PrintLine("return _Model.", Property(ctx), ";");
                        gen.Out();
                        gen.PrintLine("}"); // get


                        gen.PrintLine("set");
                        gen.PrintLine("{");
                        gen.In();
                        gen.PrintLine("_Model.", Property(ctx), " = value;");

                        gen.PrintLine();

                        // 有Presenter同步到View时, 调用通知
                        if (HasPropertyChangedNotify(ctx))
                        {
                            gen.PrintLine("if ( ", Event(ctx), " != null )");
                            gen.PrintLine("{");
                            gen.In();
                            gen.PrintLine(Event(ctx), "();");
                            gen.Out();
                            gen.PrintLine("}"); // set
                        }


                        gen.Out();
                        gen.PrintLine("}"); // set


                        gen.Out();
                        gen.PrintLine("}"); // Property
                    }
                    break;
                default:
                    return;
            }

            gen.PrintLine();

        }

        static void PropertyInit(CodeGenerator gen, DataContext rootContext, DataContext ctx)
        {
            if (ctx.Type == WidgetType.ScrollRect)
            {
                gen.PrintLine(Property(ctx), "Collection = new Framework.ObservableCollection<int, ", ClassName(ctx), ">();");
            }
        }


        #endregion

        #region Command

        public static string Command(DataContext ctx)
        {
            return "Cmd_" + ctx.Name;
        }

        static bool IsCommand( DataContext ctx )
        {
            return ctx.Type == WidgetType.Button;
        }

        static void CommandBody( CodeGenerator gen, DataContext rootContext, DataContext ctx )
        {
            if (!IsCommand(ctx))
            {
                return;
            }

            // Presenter类已经存在函数了, 不再生成
            if (IsStringExists(rootContext, Command(ctx)) )
            {
                return;
            }

            gen.PrintLine("public void ", Command(ctx), "()");
            gen.PrintLine("{");
            gen.In();

            gen.PrintLine();

            gen.Out();
            gen.PrintLine("}");
        }


        static bool IsStringExists( DataContext rootContext, string str )
        {
            var manPresenterFile = string.Format("Assets/Script/Presenter/{0}/{1}.cs", rootContext.Name, ClassName(rootContext));
            if (!File.Exists(manPresenterFile))
                return false;

            var allCodeLine = File.ReadAllLines(manPresenterFile);
            foreach( var line in allCodeLine )
            {
                if (line.Contains(str))
                {
                    return true;
                }
            }

            return false;

        }

        #endregion

        #region Network

        static void NetworkDeclare(CodeGenerator gen, gamedef.CodeGenPeer peer)
        {
            gen.PrintLine("NetworkPeer ", NetworkPeerInstance(peer), ";");
            gen.PrintLine();
        }

        static string NetworkPeerInstance(gamedef.CodeGenPeer peer)
        {
            return "_" + peer.Name + "Peer";
        }

        static string NetworkCallback(gamedef.CodeGenPeer peer, string msgType)
        {
            var prefix = "Msg_" + peer.Name + "_";

            var namepack = msgType.Split('.');
            if (namepack.Length == 2)
            {
                var rawName = namepack[1];
                return prefix + rawName;
            }

            return prefix + msgType;
        }

        static void NetworkRegisterBody(CodeGenerator gen, gamedef.CodeGenPeer peer)
        {
            gen.PrintLine(NetworkPeerInstance(peer), " = PeerManager.Instance.Get(\"", peer.Name, "\");");
            gen.PrintLine();

            foreach (string msgType in peer.RecvMessage)
            {
                gen.PrintLine(NetworkPeerInstance(peer), ".RegisterMessage<", msgType, ">( obj =>");
                gen.PrintLine("{");
                gen.In();

                gen.PrintLine(NetworkCallback(peer, msgType), "( ", NetworkPeerInstance(peer), ", obj as ", msgType, ");");

                gen.Out();
                gen.PrintLine("});");

                gen.PrintLine();
            }


        }

        static void NetworkCallbackBody(CodeGenerator gen, DataContext rootContext, gamedef.CodeGenPeer peer, string msgType)
        {

            // Presenter类已经存在函数了, 不再生成           
            if (IsStringExists(rootContext, NetworkCallback(peer, msgType)))
            {
                return;
            }

            gen.PrintLine("public void ", NetworkCallback(peer, msgType), "( NetworkPeer peer, ", msgType, " msg )");
            gen.PrintLine("{");
            gen.In();

            gen.PrintLine();

            gen.Out();
            gen.PrintLine("}");

            gen.PrintLine();
        }
        #endregion


        static void ModelInit(CodeGenerator gen, DataContext rootContext, gamedef.CodeGenModule module)
        {
            switch( module.ModelGen )
            {
                case gamedef.ModelGenType.MGT_Singleton:
                    {
                        gen.PrintLine("_Model = Framework.ModelManager.Instance.Get<", ModelTemplate.ClassName(rootContext), ">();");
                        gen.PrintLine();
                    }
                    break;
                case gamedef.ModelGenType.MGT_Instance:
                    {
                        gen.PrintLine("_Model = new ", ModelTemplate.ClassName(rootContext), "();");
                        gen.PrintLine();
                    }
                    break;
            }


            
        }



        public static void ClassBody(CodeGenerator gen, DataContext rootContext, List<DataContext> propContextList, gamedef.CodeGenModule module)
        {
            gen.PrintLine("// Generated by github.com/davyxu/cellorigin");
            gen.PrintLine("using UnityEngine;");
            gen.PrintLine("using UnityEngine.UI;");
            gen.PrintLine("using System;");
            gen.PrintLine();

            gen.PrintLine("partial class ", ClassName(rootContext), " : Framework.BasePresenter");
            gen.PrintLine("{");
            gen.In();

            gen.PrintLine(ModelTemplate.ClassName(rootContext), " _Model;");
            gen.PrintLine();

            // 网络Peer声明
            foreach (gamedef.CodeGenPeer peer in module.Peer)
            {
                NetworkDeclare(gen, peer);
            }

            if ( module.ModelGen != gamedef.ModelGenType.MGT_Manual )
            {
                // 属性声明
                foreach (DataContext propContext in propContextList)
                {
                    PropertyBody(gen, rootContext, propContext);
                }
            }


            gen.PrintLine("public void Init( )");
            gen.PrintLine("{");
            gen.In();


            ModelInit(gen, rootContext, module);

            if (module.ModelGen != gamedef.ModelGenType.MGT_Manual)
            {
                foreach (DataContext propContext in propContextList)
                {
                    PropertyInit(gen, rootContext, propContext);
                }
            }
            

            // 网络获取及消息绑定


            foreach (gamedef.CodeGenPeer peer in module.Peer)
            {
                NetworkRegisterBody(gen, peer);
            }
            

            gen.Out();
            gen.PrintLine("}"); // Bind
            gen.PrintLine();

            foreach (DataContext propContext in propContextList)
            {
                CommandBody(gen, rootContext, propContext);
            }

            foreach (gamedef.CodeGenPeer peer in module.Peer)
            {
                foreach(string msgType in peer.RecvMessage )
                {
                    NetworkCallbackBody(gen, rootContext, peer, msgType);
                }
                
            }

            gen.Out();
            gen.PrintLine("}"); // Class
        }

        #region IO

        public static string FullFileName(DataContext ctx)
        {
            return string.Format("Assets/Script/Presenter/{0}/{1}.codegen.cs", ctx.Name, ClassName(ctx));
        }

        public static void Delete(DataContext rootContext)
        {
            var file = FullFileName(rootContext);
            if (File.Exists(file))
            {
                File.Delete(file);
            }

        }

        public static void Save(CodeGenerator gen, DataContext rootContext)
        {
            CodeUtility.WriteFile(FullFileName(rootContext), gen.ToString());
        }

        #endregion

    }
}
