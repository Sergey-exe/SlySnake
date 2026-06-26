mergeInto(LibraryManager.library, {
    ShowBrowserAlert: function(message) {
        var msg = UTF8ToString(message);
        if (typeof window !== 'undefined') window.alert(msg);
        else if (typeof top !== 'undefined') top.alert(msg);
    },

    ShowBrowserConfirm: function(message, gameObjectName, methodName) {
        var msgStr = UTF8ToString(message);
        var goName = UTF8ToString(gameObjectName);
        var mName = UTF8ToString(methodName);
        
        var result = false;
        if (typeof window !== 'undefined' && window.confirm) {
            result = window.confirm(msgStr);
        } else if (typeof top !== 'undefined' && top.confirm) {
            result = top.confirm(msgStr);
        }
        
        var val = result ? 1 : 0;

        // Поочередно проверяем все возможные инстансы PluginYG2 и Unity
        if (typeof myGameInstance !== 'undefined') {
            myGameInstance.SendMessage(goName, mName, val);
        } else if (typeof unityInstance !== 'undefined') {
            unityInstance.SendMessage(goName, mName, val);
        } else if (typeof Module !== 'undefined' && Module.yg2Instance) {
            // Специфичное для некоторых версий YG2 внутреннее хранилище
            Module.yg2Instance.SendMessage(goName, mName, val);
        } else if (typeof gameInstance !== 'undefined') {
            gameInstance.SendMessage(goName, mName, val);
        } else {
            console.error("Инстанс Unity WebGL не найден! Пробуем отправить напрямую в Module...");
            try {
                // Последний резервный вариант для WebGL-шаблонов
                Module.SendMessage(goName, mName, val);
            } catch(e) {
                console.error("Критическая ошибка: не удалось найти точку входа для SendMessage.", e);
            }
        }
    }
});
