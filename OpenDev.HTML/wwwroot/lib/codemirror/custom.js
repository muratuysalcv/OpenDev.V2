var editorList = [];
function synonyms(cm, option) {
  return new Promise(function (accept) {
    setTimeout(function () {
      var cursor = cm.getCursor(), line = cm.getLine(cursor.line)
      var start = cursor.ch, end = cursor.ch
      while (start && /\w/.test(line.charAt(start - 1))) --start
      while (end < line.length && /\w/.test(line.charAt(end))) ++end
      var word = line.slice(start, end).toLowerCase()
      for (var i = 0; i < comp.length; i++) if (comp[i].indexOf(word) != -1)
        return accept({
          list: comp[i],
          from: CodeMirror.Pos(cursor.line, start),
          to: CodeMirror.Pos(cursor.line, end)
        })
      return accept(null)
    }, 100)
  })
}
// MODE LIST SQL - HTML - CSS - JAVASCRIPT - JSON
function bindCodeMirror(elementId, mode) {

  var languageCode = $('#G_CURRENT_CULTURE').val();
  var element = document.getElementById(elementId);
  var editor = {};
  var parentElementId = $('#' + elementId).parent().attr('id');
  loader("#" + parentElementId, "show");

  var modeRaw = mode;
  var f5Command = $('#' + elementId).attr('f5-command');
  var hintObj = null;
  if (mode == "sql") { mode = "text/x-mssql"; hintObj = CodeMirror.hint.sql }
  else if (mode == "javascript") { mode = "text/javascript"; hintObj = CodeMirror.hint.javascript }
  else if (mode == "css") { mode = "text/css"; hintObj = CodeMirror.hint.css }
  else if (mode == "json") { mode = "text/x-mssql"; hintObj = CodeMirror.hint.sql }
  else { mode = "text/html"; hintObj = CodeMirror.hint.html; }
  if (modeRaw == "html") {
    mode = {
      name: "htmlmixed",
      tags: {
        style: [['type", /^text/(x-)?scss$/,"text/x-scss'],
        [null, null, "css"]],
        custom: [[null, null, "customMode"]]
      }
    };
  }
  var extraKeysObject = null;
  if (f5Command == null) {

    extraKeysObject = {
      "F11": function (cm) {
        cm.setOption("fullScreen", !cm.getOption("fullScreen"));
      },
      "Esc": function (cm) {
        if (cm.getOption("fullScreen")) cm.setOption("fullScreen", false);
      },
      "Alt-F": "findPersistent",
      "Ctrl-Space": "autocomplete",
      "Ctrl-D": autoFormatSelection,
      "Cmd-D": autoFormatSelection,
      "Ctrl-S": function (cm) {
        debugger;
        var contentId = cm.getTextArea().id;
        var loaderId = $('#' + contentId).closest('div').attr('id');
        loader('#' + loaderId, 'show');

        var contenttext = cm.getValue();
        var tableName = GetObjectNameByCurrentUrl();
        var valueList = GetQueryValuesByCurrentUrl();

        valueList.push({ 'value': '[table=]' + tableName });
        valueList.push({ 'value': '[contentText=]' + contenttext });
        valueList.push({ 'value': '[inputId=]' + contentId });
        valueList.push({ 'value': '[requestName=]' + 'UpdateField' });

        $.ajax({
          type: "POST",
          url: '/' + languageCode + '/api/webapi/IDE',
          data: valueList,
          success: function (result) {
            loader('#' + loaderId, 'hide');
            if (result.SUCCESS == 'SUCCESS') {
              uiAlertSuccess('SUCCESS', result.MESSAGE);
            }
            else {
              uiAlertError('ERROR', result.MESSAGE);
            }
          },
          dataType: 'JSON'
        });
      },
      'Cmd-K': 'toggleComment',
      'Ctrl-K': 'toggleComment',
      "Ctrl-Q": function (cm) { cm.foldCode(cm.getCursor()); }
    };
  }
  else {
    extraKeysObject = {
      "F11": function (cm) {
        cm.setOption("fullScreen", !cm.getOption("fullScreen"));
      },
      "Esc": function (cm) {
        if (cm.getOption("fullScreen")) cm.setOption("fullScreen", false);
      },
      "Alt-F": "findPersistent",
      "Ctrl-Space": "autocomplete",
      "Ctrl-D": autoFormatSelection,
      "Cmd-D": autoFormatSelection,
      "Ctrl-S": function (cm) {
        debugger;
        var contentId = cm.getTextArea().id;
        var contenttext = cm.getValue();
        var tableName = GetObjectNameByCurrentUrl();
        var valueList = GetQueryValuesByCurrentUrl();

        valueList.push({ key: 'table', value: tableName });
        valueList.push({ key: 'contentText', value: contenttext });
        valueList.push({ key: 'inputId', value: contentId });
        valueList.push({ key: 'RequestName', value: "UpdateField" });

        $.ajax({
          type: "POST",
          url: '/' + languageCode + '/api/webapi/IDE',
          data: valueList,
          success: function (result) {

          },
          dataType: 'JSON'
        });
      },
      'Cmd-K': 'toggleComment',
      'Ctrl-K': 'toggleComment',
      "Ctrl-Q": function (cm) { cm.foldCode(cm.getCursor()); }
    };
  }

  // Opendev Keywords Highlight
  editor = CodeMirror.fromTextArea(element, {
    mode: mode,
    theme: "monokai",
    lineNumbers: true,
    foldGutter: true,
    gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter", "CodeMirror-lint-markers"],
    lint: true,
    autoCloseTags: true,
    highlightSelectionMatches: { showToken: /\w/, annotateScrollbar: true },
    matchBrackets: true,
    autoCloseBrackets: true,
    matchTags: { bothTags: true },
    extraKeys: extraKeysObject,
    keyMap: "sublime",
    autofocus: false,
    lineWrapping: true,
    indentWithTabs: true,
    showCursorWhenSelecting: true,
    /*hint: hintObj,*/
    smartIndent: true,
    autocorrect: true,
    change: function (cm) {
      var currentVal = cm.getValue();
      debugger;
    },
    change: function (cm) {
      var currentVal = cm.getValue();
      debugger;
    },
    update: function (cm) {
      var currentVal = cm.getValue();
      debugger;
    }

  });

  function getSelectedRange() {
    return { from: editor.getCursor(true), to: editor.getCursor(false) };
  }

  function autoFormatSelection() {
    var range = getSelectedRange();
    editor.autoFormatRange(range.from, range.to);
  }

  function commentSelection(isComment) {
    var range = getSelectedRange();
    editor.commentRange(isComment, range.from, range.to);
  }

  editor.setSize(null, 450);
  editorList.push({ ElementId: elementId, Editor: editor });
  editor.on("keyup", function (cm, event) {
    var currentEditorText = cm.getValue();
    debugger;
    if (cm.options.mode != 'text/x-mssql') {
      return;
    }
    currentEditorText = currentEditorText.replace("\\", " ");
    currentEditorText = currentEditorText.replace(";", " ");
    currentEditorText = currentEditorText.replace(")", " ");
    currentEditorText = currentEditorText.replace("(", " ");
    currentEditorText = currentEditorText.replace(",", " ");

    currentEditorText = currentEditorText.replace(/[\r\n\t]/g, " ");
    var values = currentEditorText.split(' ');
    debugger;
    var hints = []
    for (var i = 0; i < values.length; i++) {
      var val = values[i].replace(';', '');
      if (val.length > 0) {
        if (val[0] == '@') {
          hints.push(val);
        }
      }
    }
    try {
      for (var key in cm.options.hintOptions.tables) {
        if (key[0] == '@') {
          delete cm.options.hintOptions.tables[key];
        }
      }

      var currentHintTables = cm.options.hintOptions.tables;
      for (var i = 0; i < hints.length; i++) {
        var currentHint = hints[i];
        var curHintedVal = currentHintTables[currentHint];
        if (curHintedVal == null) {
          cm.options.hintOptions.tables[currentHint] = [];
        }
      }
    } catch (e) {

    }

  });

  var urlKey = '';
  var urlVal = '';
  try {
    urlKey = window.location.href.split('?')[1].split('=')[0];
    urlVal = window.location.href.split('?')[1].split('=')[1];
  } catch (e) {
    urlKey = "Key";
    urlVal = "SQL_COMMAND";
  }
  $.get("/tr-TR/api/webapi/codemirror?mode=" + modeRaw + "&" + urlKey + "=" + urlVal,
    function (data, status) {

      debugger;
      if (modeRaw == "sql") {
        editor.setOption("hintOptions", { tables: data });
      }
      else if (modeRaw == "json") {
        editor.setOption("hintOptions", { tables: data });
      }
      else if (modeRaw == "html") {
        debugger;
      }
      loader("#" + parentElementId, "hide");
    });

  editor.on('inputRead', function onChange(editor, input) {
    if (input.text[0] === ';' || input.text[0] === ' ') { return; }
    CodeMirror.commands.autocomplete(editor, null, { completeSingle: false })
  });

  // set editor content
  editor.getDoc().setValue(element.value);
}

//var MATERIAL_TYPE = {
//    ID: "ID",
//    TYPE: "TYPE"
//}
//CodeMirror.registerHelper("hints", "javascript", ["MATERIAL_TYPE"])
$(document).ready(function () {
  $(".codemirror-sql").each(function (index) {
    var mode = "sql";
    var id = $(this).attr("id");
    bindCodeMirror(id, mode);
  });
  $(".codemirror-json").each(function (index) {
    var mode = "json";
    var id = $(this).attr("id");
    bindCodeMirror(id, mode);
  });
  $(".codemirror-js").each(function (index) {
    var mode = "javascript";
    var id = $(this).attr("id");
    bindCodeMirror(id, mode);
  });
  $(".codemirror-css").each(function (index) {
    var mode = "css";
    var id = $(this).attr("id");
    bindCodeMirror(id, mode);
  });
  $(".codemirror-html").each(function (index) {
    var mode = "html";
    var id = $(this).attr("id");
    bindCodeMirror(id, mode);
  });
  $(".codemirror-javascript").each(function (index) {
    var mode = "javascript";
    var id = $(this).attr("id");
    bindCodeMirror(id, mode);
  });
})
function GetEditorByElementId(id) {
  debugger;
  for (var i = 0; i < editorList.length; i++) {
    if (editorList[i].ElementId == id.replace("#", "")) {
      return editorList[i].Editor;
    }
  }
  return null;
}