---
title: "How to use the GeologicalProperties microservice?"
output: html_document
---

Typical Usage
===
1. Upload a new GeologicalProperties using the `Post` web api method.
2. Call the `Get` method with the identifier of the uploaded GeologicalProperties as argument. 
The return Json object contains the GeologicalProperties description.
3. Optionally send a `Delete` request with the identifier of the GeologicalProperties in order to delete the GeologicalProperties if you do not 
want to keep the GeologicalProperties uploaded on the microservice.


