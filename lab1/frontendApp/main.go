package main

import (
	"html/template"
	"log"
	"net/http"
	"path/filepath"
	"net/url"
	"strings"
	"io/ioutil"
	"strconv"
	"encoding/json"
)

func redirect(w http.ResponseWriter, r *http.Request) {

	http.Redirect(w, r, "http://www.google.com", 301)
}

func renderTemplate(w http.ResponseWriter, r *http.Request, t string, data interface{}) {
	lp := filepath.Join("templates", "layout.html")
	fp := filepath.Join("templates", t)

	tmpl, _ := template.ParseFiles(lp, fp)
	tmpl.Execute(w, &data)
}

type UserData struct {
	Email string
	Password string
}

func indexAction(w http.ResponseWriter, r *http.Request) {
	renderTemplate(w, r, "index.html", nil)
}

func registerAction(w http.ResponseWriter, r *http.Request) {
	r.ParseForm()
	userId := processAddUser(r.Form.Get("email"), r.Form.Get("password"))
	userData := processGetUser(userId)

	renderTemplate(w, r, "result.html", &userData)
}

func processAddUser(email string, password string) int {
	data := url.Values{
		"Email": {email},
		"Password": {password},
	}

	client := &http.Client{}

	request, _ := http.NewRequest("PUT", "http://localhost:9000/api/user/", strings.NewReader(data.Encode()))
	request.Header.Add("Content-Type", "application/x-www-form-urlencoded")
	response, _ := client.Do(request)
	defer response.Body.Close()

	body, _ := ioutil.ReadAll(response.Body)
	userId, _ := strconv.ParseInt(string(body), 10, 64)

	return int(userId)
}

func processGetUser(userId int) UserData {
	response, _ := http.Get("http://localhost:9000/api/user/" + strconv.Itoa(userId))
	defer response.Body.Close()

	var data UserData
	json.NewDecoder(response.Body).Decode(&data)

	return data
}

func main() {
	http.HandleFunc("/", indexAction)
	http.HandleFunc("/register", registerAction)
	err := http.ListenAndServe(":9090", nil)
	if err != nil {
		log.Fatal("ListenAndServe: ", err)
	}
}