mergeInto(LibraryManager.library, {

  IntSdk: function (appid,appName,serverUrl,debug) {

   let _appName = Pointer_stringify(appName);
   let _serverUrl = Pointer_stringify(serverUrl);
   console.log("_appName",_appName);
   console.log("_serverUrl",_serverUrl);
   console.log("debug",UTF8ToString(debug));
   let _debug = debug == 1 ? true : false;

    console.log("[StatsWx] init config",appid,_appName,_serverUrl,_debug);

    window.sd.setPara({
      appid: appid,  // 应用/游戏ID
      name: _appName,
      server_url: _serverUrl,
      // 全埋点控制开关
      autoTrack: {
          appLaunch: true, // MPL 事件采集，默认为 true
          appShow: true, // MPV 事件采集，默认为 true
          appHide: true, // MPQ 事件采集，默认为 true
      },
      show_log: _debug   //是否打印日志，默认为 false
     });

     sd.init();
  },
  
  Login: function (uid,opeid) {
    let _uid = Pointer_stringify(uid);
    let _opeid = Pointer_stringify(opeid);
    console.log('[StatsWx] trackLogin uid', _uid);
    console.log('[StatsWx] trackLogin openId', _opeid);

    sd.login(_uid);
    sd.setOpenid(_opeid);
  },
  
  Track: function (type,openId,param,custom) {
    let _type = Pointer_stringify(type);
    let _openId = Pointer_stringify(openId);
    let _param = Pointer_stringify(param);
    let _custom = Pointer_stringify(custom);

    let config = {
      user: { openid: _openId },
      param: _param ? [{ url_query : _param }] : [],
      custom: _custom ? [_custom] : [],
    };
    console.log(`[StatsWx] track type:${type}`, config);
    sd.track(_type, config);
  },
  

  GetUserUUID: function (str) {
    var returnStr = sd.getUserUUID();
    var buffer = _malloc(lengthBytesUTF8(returnStr) + 1);
    writeStringToMemory(returnStr, buffer);
    return buffer;
  },
});