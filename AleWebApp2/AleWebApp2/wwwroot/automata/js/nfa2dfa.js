/*jslint browser: true*/
/*global window, regexToNfa, nfaToDfa, genAutomataSVG, $*/

$(document).ready(function () {
    'use strict';

    function b64EncodeUnicode(str) {
        return window.btoa(encodeURIComponent(str).replace(/%([0-9A-F]{2})/g, function (match, p1) {
            match = match.prototype; // For jslint.
            return String.fromCharCode('0x' + p1);
        }));
    }

    function b64DecodeUnicode(str) {
        return decodeURIComponent(Array.prototype.map.call(window.atob(str.replace(' ', '+')), function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
    }

    function getParameterByName(name) {
        var url = window.location.href,
            regex,
            results;
        name = name.replace(/[\[\]]/g, "\\$&");
        regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)");
        results = regex.exec(url);
        if (!results) {
            return null;
        }
        if (!results[2]) {
            return '';
        }
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }

    function toNature(col) {
        var i,
            j,
            base = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ',
            result = 0;
        for (i = 0, j = col.length - 1; i < col.length; i += 1, j -= 1) {
            result += Math.pow(base.length, j) * (base.indexOf(col[i]) + 1);
        }
        return result;
    }

    function showDfaTable(start) {
        var i,
            j,
            states = {},
            nodes = [],
            stack = [start],
            symbols = [],
            top,
            html = '';
        while (stack.length > 0) {
            top = stack.pop();
            if (!states.hasOwnProperty(top.id)) {
                states[top.id] = top;
                top.nature = toNature(top.id);
                nodes.push(top);
                for (i = 0; i < top.edges.length; i += 1) {
                    if (top.edges[i][0] !== 'ϵ' && symbols.indexOf(top.edges[i][0]) < 0) {
                        symbols.push(top.edges[i][0]);
                    }
                    stack.push(top.edges[i][1]);
                }
            }
        }
        nodes.sort(function (a, b) {
            return a.nature - b.nature;
        });
        symbols.sort();
        html += '<table class="table">';
        html += '<thead>';
        html += '<tr>';
        html += '<th>NFA STATE</th>';
        html += '<th>DFA STATE</th>';
        for (i = 0; i < symbols.length; i += 1) {
            html += '<th>' + symbols[i] + '</th>';
        }
        html += '</tr>';
        html += '</thead>';
        html += '<tbody>';
        for (i = 0; i < nodes.length; i += 1) {
            html += '<tr>';
            html += '<td>{' + nodes[i].key + '}</td>';
            html += '<td>' + nodes[i].id + '</td>';
            for (j = 0; j < symbols.length; j += 1) {
                html += '<td>';
                if (nodes[i].trans.hasOwnProperty(symbols[j])) {
                    html += nodes[i].trans[symbols[j]].id;
                }
                html += '</td>';
            }
            html += '</tr>';
        }
        html += '</tbody>';
        html += '</table>';
        $('#dfa_table').html(html);
    }

    $('#button_convert').click(function () {
        var start = regexToNfa($('#input_regex').val()),
            prefix = window.location.href.split('?')[0] + '?regex=',
            input = b64EncodeUnicode($('#input_regex').val());
        $('#input_url').val(prefix + input);
        $('#alert_error').hide();
        if (typeof start === 'string') {
            $('#p_error').text(start);
            $('#alert_error').show();
        } else {
            start = nfaToDfa(start);
            showDfaTable(start);
            $('svg').attr("width", $('svg').parent().width());
            genAutomataSVG('svg', start);
        }
    });

    var input = getParameterByName('regex');
    if (input) {
        input = b64DecodeUnicode(input);
        $('#input_regex').val(input);
        $('#button_convert').click();
    }

});
