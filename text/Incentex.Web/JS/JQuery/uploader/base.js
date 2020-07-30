/*
    Functions from prototype.js (http://prototype.conio.net/)
    Rewritten just a little bit.
*/

/*function $() {
    for ( var i = 0, elements = []; i < arguments.length; ++i )
        elements.push ( typeof arguments[i] == 'string' ? document.getElementById ( arguments[i] ) : arguments[i] );
    return elements.length == 1 ? elements[0] : elements;
}*/

function $A ( array ) {
    if ( !array ) return [];
    if ( array.toArray ) return array.toArray();
    for ( var i = 0, result = []; i < array.length; ++i )
        result.push ( array[i] );
    return result;
}

var Class = {
    create: function() { return function() { this.initialize.apply ( this, arguments ); } }
}

Object.extend = function ( dest, source ) {
    for ( p in source ) dest[p] = source[p];
    return dest;
}

Function.prototype.bind = function() {
    var __method = this, args = $A(arguments), obj = args.shift();
    return function() { return __method.apply ( obj, args.concat ( $A ( arguments ) ) ); }
}

Function.prototype.bindAsEventListener = function ( object ) {
    var __method = this;
    return function ( event ) { return __method.call ( object, event || window.event ); }
}



var Try = {
    these: function() {
        for ( var i = 0, retval; i < arguments.length; ++i )
            try { retval = arguments[i](); break; } catch(error){}
        return retval;
    }
}


if (!window.Event) {
    var Event = new Object();
}

Object.extend(Event, {
    KEY_BACKSPACE: 8,
    KEY_TAB:       9,
    KEY_RETURN:   13,
    KEY_ESC:      27,
    KEY_LEFT:     37,
    KEY_UP:       38,
    KEY_RIGHT:    39,
    KEY_DOWN:     40,
    KEY_DELETE:   46,

    element: function(event) {
        return event.target || event.srcElement;
    },

    isLeftClick: function(event) {
        return (((event.which) && (event.which == 1)) || ((event.button) && (event.button == 1)));
    },

    pointerX: function(event) {
        return event.pageX || (event.clientX + (document.documentElement.scrollLeft || document.body.scrollLeft));
    },

    pointerY: function(event) {
        return event.pageY || (event.clientY + (document.documentElement.scrollTop || document.body.scrollTop));
    },

    stop: function(event) {
        if (event.preventDefault) {
            event.preventDefault();
            event.stopPropagation();
        } else {
            event.returnValue = false;
            event.cancelBubble = true;
        }
    },

    observers: false,

    _observeAndCache: function(element, name, observer, useCapture) {
        if (!this.observers) this.observers = [];
        if (element.addEventListener) {
            this.observers.push([element, name, observer, useCapture]);
            element.addEventListener(name, observer, useCapture);
        } else if (element.attachEvent) {
            this.observers.push([element, name, observer, useCapture]);
            element.attachEvent('on' + name, observer);
        }
    },

    unloadCache: function() {
        if (!Event.observers) return;
        for (var i = 0; i < Event.observers.length; i++) {
            Event.stopObserving.apply(this, Event.observers[i]);
            Event.observers[i][0] = null;
        }
        Event.observers = false;
    },

    observe: function(element, name, observer, useCapture) {
        var element = $(element);
        useCapture = useCapture || false;
        if (name == 'keypress' && (navigator.appVersion.match(/Konqueror|Safari|KHTML/) || element.attachEvent))
            name = 'keydown';
        this._observeAndCache(element, name, observer, useCapture);
    },

    stopObserving: function(element, name, observer, useCapture) {
        var element = $(element);
        useCapture = useCapture || false;

        if (name == 'keypress' && (navigator.appVersion.match(/Konqueror|Safari|KHTML/) || element.detachEvent))
            name = 'keydown';

        if (element.removeEventListener) {
            element.removeEventListener(name, observer, useCapture);
        } else if (element.detachEvent) {
            element.detachEvent('on' + name, observer);
        }
    }
});
