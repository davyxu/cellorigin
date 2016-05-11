﻿// Generated by github.com/davyxu/cellorigin
using UnityEngine;
using UnityEngine.UI;
using System;

partial class LoginPresenter : Framework.BasePresenter
{
	LoginModel _Model;
	
	NetworkPeer _loginPeer;
	
	NetworkPeer _gamePeer;
	
	public Action OnAccountChanged;
	public string Account
	{
		get
		{
			return _Model.Account;
		}
		set
		{
			_Model.Account = value;
			
			if ( OnAccountChanged != null )
			{
				OnAccountChanged();
			}
		}
	}
	
	public Action OnAddressChanged;
	public string Address
	{
		get
		{
			return _Model.Address;
		}
		set
		{
			_Model.Address = value;
			
			if ( OnAddressChanged != null )
			{
				OnAddressChanged();
			}
		}
	}
	
	public Framework.ObservableCollection<int, LoginServerInfoPresenter> LoginServerList { get; set; }
	
	public void Init( )
	{
		_Model = Framework.ModelManager.Instance.Get<LoginModel>();
		
		LoginServerList = new Framework.ObservableCollection<int, LoginServerInfoPresenter>();
		_loginPeer = PeerManager.Instance.Get("login");
		
		_loginPeer.RegisterMessage<gamedef.PeerConnected>( obj =>
		{
			Msg_login_PeerConnected( _loginPeer, obj as gamedef.PeerConnected);
		});
		
		_loginPeer.RegisterMessage<gamedef.LoginACK>( obj =>
		{
			Msg_login_LoginACK( _loginPeer, obj as gamedef.LoginACK);
		});
		
		_gamePeer = PeerManager.Instance.Get("game");
		
		_gamePeer.RegisterMessage<gamedef.PeerConnected>( obj =>
		{
			Msg_game_PeerConnected( _gamePeer, obj as gamedef.PeerConnected);
		});
		
		_gamePeer.RegisterMessage<gamedef.VerifyGameACK>( obj =>
		{
			Msg_game_VerifyGameACK( _gamePeer, obj as gamedef.VerifyGameACK);
		});
		
	}
	
}
