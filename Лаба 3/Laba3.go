package main

import (
	"azul3d.org/engine/keyboard"
	_ "azul3d.org/engine/keyboard"
	"fmt"
	"math/rand"
	"sync"
	"time"
)

var product = 200 //слишком мало

const producerCount int = 3
const consumerCount int = 2

func produce(link chan<- int, wg *sync.WaitGroup) {
	defer wg.Done()
	for product <= 80 {
		product += 1 + rand.Intn(100)
		link <-product
	}
	for product >=100 {
		time.Sleep(100) // можно/нужно изменить на значения поменьше, иначе консьюмеры успевают съесть абсолютно всё 
	}
}

func consume(link <-chan int, wg *sync.WaitGroup) {
	defer wg.Done()
	for product == 0 {
		time.Sleep(100)
	}
	i := 0
	for product >0{
		product--
		i++
		fmt.Printf("Gulp %d\n", i)
	}

}

func main() {
	link := make(chan int)
	wp := &sync.WaitGroup{}
	wc := &sync.WaitGroup{}

	wp.Add(producerCount)
	wc.Add(consumerCount)

	for i := 0; i < producerCount; i++ {
		go produce(link, wp)
	}

	for i := 0; i < consumerCount; i++ {
		go consume(link, wc)
	}
	watcher := keyboard.NewWatcher()
	status := watcher.States()
	q := status[keyboard.Q]
	if q == keyboard.Down{
		close(link)
	}
	wp.Wait()

	wc.Wait()
}
