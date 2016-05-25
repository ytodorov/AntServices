﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="SmartAdminMvc._404" %>
<% Response.StatusCode = 404; %>

<!DOCTYPE html>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">

        <title>Page Not Found</title>

        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700' rel='stylesheet' type='text/css'>
        <link href='/bundles/font-awesome/css' rel='stylesheet' type='text/css'>
        <link href='/bundles/animate/css' rel='stylesheet' type='text/css'>
        <link href='/bundles/bootstrap/css' rel='stylesheet' type='text/css'>
        <link href='/bundles/peicon7stroke/css' rel='stylesheet' type='text/css'>
        <link href='/bundles/homer/css' rel='stylesheet' type='text/css'>
    </head>
    <body class="blank">

        <!--[if lt IE 7]>
        <p class="alert alert-danger">You are using an <strong>outdated</strong> browser. Please <a href="http://browsehappy.com/">upgrade your browser</a> to improve your experience.</p>
        <![endif]-->

        <div class="color-line"></div>
        <div class="error-container">
            <i class="pe-7s-way text-success big-icon"></i>
            <h1>404</h1>
            <strong>Page Not Found</strong>
            <p>
                Sorry, but the page you are looking for has note been found. Try checking the URL for error, then hit the refresh button on your browser or try found something else in our app.

            </p>
            <a href="/" class="btn btn-xs btn-success">Go back to home</a>
        </div>

    </body>
</html>
