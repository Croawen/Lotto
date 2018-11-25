(function () {
    $(function () {
        var template = '<div class="api-auth-container">' +
            '<form>' +
            '<input type="text" placeholder="email" name="email" required value="">' +
            '<input type="password" placeholder="password" name="password" required value="">' +
            '<button class="get-token">Get token</button>' +
            '</form>' +
            '<div><textarea id="apiKey" rows="8" cols="56" placeholder="JWT token"></textarea></div>' +
            '</div>';

        $('#swagger-ui-container .info').append($(template));

        $('.get-token').click(function (e) {
            e.preventDefault();

            $.ajax({
                url: '/auth/login',
                type: 'POST',
                data: JSON.stringify({
                    grantType: 'password',
                    email: $('input[name="email"]').val(),
                    password: $('input[name="password"').val()
                }),
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    $('#apiKey').val(data.access_token);
                    setHeaderToken(data.access_token);
                },
                error: function (error) { alert(error.statusText); }
            });
        });

        $('#apiKey').change(function () {
            var key = $(this).val();
            setHeaderToken(key);
        });

        function setHeaderToken(key) {
            console.info('Set bearer token to: ' + key);

            swaggerUi.api.clientAuthorizations.add("key", new SwaggerClient.ApiKeyAuthorization("Authorization", "Bearer " + key, "header"));
        }
    });
})();